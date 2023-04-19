using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Dapper;
using Turismo.Models;

namespace Turismo.Repositories
{
    internal class TurismoRepository : ITurismoRepository
    {
        private string _connection { get; set; }

        public TurismoRepository()
        {
            _connection = ConfigurationManager.ConnectionStrings["Turismo"].ConnectionString;
        }

        public int InserirCidade(Cidade cidade)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"INSERT INTO Cidade (Nome) VALUES ('{cidade.Nome}');");
            sb.Append("SELECT CAST(scope_identity() AS INT)");

            SqlConnection db = new SqlConnection(_connection);
            db.Open();
            int id = Convert.ToInt32(db.ExecuteScalar(sb.ToString()));
            db.Close();

            return id;
        }

        public int InserirEndereco(Endereco endereco)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO Endereco (Logradouro, Numero, Bairro, CEP, Complemento, Id_Cidade, Data_Cadastro) VALUES (");
            sb.Append($"'{endereco.Logradouro}', ");
            sb.Append($"{endereco.Numero}, ");
            sb.Append($"'{endereco.Bairro}', ");
            sb.Append($"'{endereco.CEP}', ");
            sb.Append($"'{endereco.Complemento}', ");
            sb.Append($"{InserirCidade(endereco.Cidade)}, ");
            sb.Append($"'{endereco.DataCadastro.ToString("MM/dd/yyyy hh:mm:ss")}');");
            sb.Append("SELECT CAST(scope_identity() AS INT)");

            SqlConnection db = new SqlConnection(_connection);
            db.Open();
            int id = Convert.ToInt32(db.ExecuteScalar(sb.ToString()));
            db.Close();

            return id;
        }

        public int InserirCliente(Cliente cliente)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO Cliente (Nome, Telefone, Id_Endereco, Data_Cadastro) VALUES (");
            sb.Append($"'{cliente.Nome}', ");
            sb.Append($"'{cliente.Telefone}', ");
            sb.Append($"{InserirEndereco(cliente.Endereco)}, ");
            sb.Append($"'{cliente.DataCadastro.ToString("MM/dd/yyyy hh:mm:ss")}')");
            sb.Append("SELECT CAST(scope_identity() AS INT)");

            SqlConnection db = new SqlConnection(_connection);
            db.Open();
            int id = Convert.ToInt32(db.ExecuteScalar(sb.ToString()));
            db.Close();

            return id;
        }

        public int InserirPassagem(Passagem passagem)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO Passagem (Id_Origem, Id_Destino, Id_Cliente, Data, Valor) VALUES (");
            sb.Append($"{InserirEndereco(passagem.Origem)}, ");
            sb.Append($"{InserirEndereco(passagem.Destino)}, ");
            sb.Append($"{InserirCliente(passagem.Cliente)}, ");
            sb.Append($"'{passagem.Data.ToString("MM/dd/yyyy hh:mm:ss")}', ");
            sb.Append($"{passagem.Valor.ToString(CultureInfo.InvariantCulture)})");
            sb.Append("SELECT CAST(scope_identity() AS INT)");

            SqlConnection db = new SqlConnection(_connection);
            db.Open();
            int id = Convert.ToInt32(db.ExecuteScalar(sb.ToString()));
            db.Close();

            return id;
        }

        public int InserirHotel(Hotel hotel)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO Hotel (Nome, Id_Endereco, Data_Cadastro, Valor) VALUES (");
            sb.Append($"'{hotel.Nome}', ");
            sb.Append($"{InserirEndereco(hotel.Endereco)}, ");
            sb.Append($"'{hotel.DataCadastro.ToString("MM/dd/yyyy hh:mm:ss")}', ");
            sb.Append($"{hotel.Valor.ToString(CultureInfo.InvariantCulture)});");
            sb.Append("SELECT CAST(scope_identity() AS INT)");
            SqlConnection db = new SqlConnection(_connection);
            db.Open();
            int id = Convert.ToInt32(db.ExecuteScalar(sb.ToString()));
            db.Close();
            return id;
        }

        public void AtualizarCampo(int id, string tabela, string campo, string atualizarString)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"UPDATE {tabela} SET {campo} = '{atualizarString}' WHERE Id = {id}");

            SqlConnection db = new SqlConnection(_connection);
            db.Open();
            db.Execute(sb.ToString());
            db.Close();
        }

        public void RemoverPacote(int id)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"DELETE FROM Pacote WHERE Id = {id}");

            SqlConnection db = new SqlConnection(_connection);
            db.Open();
            db.Execute(sb.ToString());
            db.Close();
        }

        public void Inserir(Pacote pacote)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO Pacote (Id_Hotel, Id_Passagem, Data_Cadastro, Valor, Id_Cliente) VALUES (");
            sb.Append($"{InserirHotel(pacote.Hotel)}, ");
            sb.Append($"{InserirPassagem(pacote.Passagem)}, ");
            sb.Append($"'{pacote.DataCadastro.ToString("MM/dd/yyyy hh:mm:ss")}', ");
            sb.Append($"{pacote.Valor.ToString(CultureInfo.InvariantCulture)}, ");
            sb.Append($"{InserirCliente(pacote.Cliente)})");

            SqlConnection db = new SqlConnection(_connection);
            db.Open();
            db.Execute(sb.ToString());
            db.Close();
        }

        public List<Pacote> FindAll()
        {
            List<Pacote> pacotes = new List<Pacote>();

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Id, Id_Hotel, Id_Passagem, Data_Cadastro, Valor, Id_Cliente FROM Pacote");

            SqlConnection db = new SqlConnection(_connection);
            db.Open();

            IDataReader dr = db.ExecuteReader(sb.ToString());

            while (dr.Read())
            {
                Pacote pacote = new Pacote();

                pacote.Id = Convert.ToInt32(dr["Id"]);
                pacote.DataCadastro = DateTime.Parse(dr["Data_Cadastro"].ToString());
                pacote.Valor = Convert.ToDecimal(dr["Valor"]);

                int idCliente = Convert.ToInt32(dr["Id_Cliente"]);
                int idPassagem = Convert.ToInt32(dr["Id_Passagem"]);
                int idHotel = Convert.ToInt32(dr["Id_Hotel"]);
                pacote.Hotel = RetornarHotel(idHotel);
                pacote.Passagem = RetornarPassagem(idPassagem);
                pacote.Cliente = RetornarCliente(idCliente);

                pacotes.Add(pacote);
            }
            dr.Close();
            db.Close();

            return pacotes;
        }

        public Hotel RetornarHotel(int id)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Id, Nome, Id_Endereco, Data_Cadastro, Valor FROM Hotel WHERE Id = " + id);

            SqlConnection db = new SqlConnection(_connection);
            db.Open();

            IDataReader dr = db.ExecuteReader(sb.ToString());

            Hotel hotel = new Hotel();

            if (dr.Read())
            {
                hotel.Id = Convert.ToInt32(dr["Id"]);
                hotel.Nome = Convert.ToString(dr["Nome"]);
                hotel.DataCadastro = DateTime.Parse(dr["Data_Cadastro"].ToString());
                hotel.Valor = Convert.ToDecimal(dr["Valor"]);

                int idEndereco = Convert.ToInt32(dr["Id_Endereco"]);
                hotel.Endereco = RetornarEndereco(idEndereco);
            }
            dr.Close();
            db.Close();

            return hotel;
        }

        public Endereco RetornarEndereco(int id)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Id, Logradouro, Numero, Bairro, CEP, Complemento, Id_Cidade, Data_Cadastro FROM Endereco WHERE Id = " + id);

            SqlConnection db = new SqlConnection(_connection);
            db.Open();

            IDataReader dr = db.ExecuteReader(sb.ToString());

            Endereco endereco = new Endereco();

            if (dr.Read())
            {
                endereco.Id = Convert.ToInt32(dr["Id"]);
                endereco.Logradouro = Convert.ToString(dr["Logradouro"]);
                endereco.Numero = Convert.ToInt32(dr["Numero"]);
                endereco.Bairro = Convert.ToString(dr["Bairro"]);
                endereco.CEP = Convert.ToString(dr["CEP"]);
                endereco.Complemento = Convert.ToString(dr["Complemento"]);
                endereco.DataCadastro = DateTime.Parse(dr["Data_Cadastro"].ToString());

                int idCidade = Convert.ToInt32(dr["Id_Cidade"]);
                endereco.Cidade = RetornarCidade(idCidade);
            }
            dr.Close();
            db.Close();

            return endereco;
        }

        public Cidade RetornarCidade(int id)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Id, Nome FROM Cidade WHERE Id = " + id);

            SqlConnection db = new SqlConnection(_connection);
            db.Open();

            IDataReader dr = db.ExecuteReader(sb.ToString());

            Cidade cidade = new Cidade();

            if (dr.Read())
            {
                cidade.Id = Convert.ToInt32(dr["Id"]);
                cidade.Nome = Convert.ToString(dr["Nome"]);
            }
            dr.Close();
            db.Close();

            return cidade;
        }

        public Cliente RetornarCliente(int id)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Id, Nome, Telefone, Id_Endereco, Data_Cadastro FROM Cliente WHERE Id = " + id);

            SqlConnection db = new SqlConnection(_connection);
            db.Open();

            IDataReader dr = db.ExecuteReader(sb.ToString());

            Cliente cliente = new Cliente();

            if (dr.Read())
            {
                cliente.Id = Convert.ToInt32(dr["Id"]);
                cliente.Nome = Convert.ToString(dr["Nome"]);
                cliente.Telefone = Convert.ToString(dr["Telefone"]);
                cliente.DataCadastro = DateTime.Parse(dr["Data_Cadastro"].ToString());

                int idEndereco = Convert.ToInt32(dr["Id_Endereco"]);
                cliente.Endereco = RetornarEndereco(idEndereco);
            }
            dr.Close();
            db.Close();

            return cliente;
        }

        public Passagem RetornarPassagem(int id)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Id, Id_Origem, Id_Destino, Id_Cliente, Data, Valor FROM Passagem WHERE Id = " + id);

            SqlConnection db = new SqlConnection(_connection);
            db.Open();

            IDataReader dr = db.ExecuteReader(sb.ToString());

            Passagem passagem = new Passagem();

            if (dr.Read())
            {
                passagem.Id = Convert.ToInt32(dr["Id"]);
                passagem.Data = DateTime.Parse(dr["Data"].ToString());
                passagem.Valor = Convert.ToDecimal(dr["Valor"]);

                int idOrigem = Convert.ToInt32(dr["Id_Origem"]);
                int idDestino = Convert.ToInt32(dr["Id_Destino"]);
                int idCliente = Convert.ToInt32(dr["Id_Cliente"]);
                passagem.Origem = RetornarEndereco(idOrigem);
                passagem.Destino = RetornarEndereco(idDestino);
                passagem.Cliente = RetornarCliente(idCliente);
            }
            dr.Close();
            db.Close();

            return passagem;
        }

    }
}
