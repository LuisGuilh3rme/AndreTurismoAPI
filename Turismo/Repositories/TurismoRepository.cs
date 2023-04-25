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
        private readonly string _identity = "SELECT CAST(scope_identity() AS INT)";
        private string _connection { get; set; }

        public TurismoRepository()
        {
            _connection = ConfigurationManager.ConnectionStrings["Turismo"].ConnectionString;
        }

        public int InserirCidade(Cidade cidade)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Cidade.INSERT);
            sb.Append(_identity);

            SqlConnection db = new SqlConnection(_connection);
            int id = Convert.ToInt32(db.ExecuteScalar(sb.ToString(), cidade));
            db.Close();

            return id;
        }

        public int InserirEndereco(Endereco endereco)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Endereco.INSERT);
            sb.Replace("@Cidade", InserirCidade(endereco.Cidade).ToString());
            sb.Append(_identity);

            SqlConnection db = new SqlConnection(_connection);
            int id = Convert.ToInt32(db.ExecuteScalar(sb.ToString(), endereco));
            db.Close();

            return id;
        }

        public int InserirCliente(Cliente cliente)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Cliente.INSERT);
            sb.Replace("@Endereco", InserirEndereco(cliente.Endereco).ToString());
            sb.Append(_identity);

            SqlConnection db = new SqlConnection(_connection);
            int id = Convert.ToInt32(db.ExecuteScalar(sb.ToString(), cliente));
            db.Close();

            return id;
        }

        public int InserirPassagem(Passagem passagem)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Passagem.INSERT);
            sb.Replace("@Origem", InserirEndereco(passagem.Origem).ToString());
            sb.Replace("@Destino", InserirEndereco(passagem.Destino).ToString());
            sb.Replace("@Cliente", InserirCliente(passagem.Cliente).ToString());
            sb.Append(_identity);

            SqlConnection db = new SqlConnection(_connection);
            int id = Convert.ToInt32(db.ExecuteScalar(sb.ToString(), passagem));
            db.Close();

            return id;
        }

        public int InserirHotel(Hotel hotel)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Hotel.INSERT);
            sb.Replace("@Endereco", InserirEndereco(hotel.Endereco).ToString());
            sb.Append(_identity);

            SqlConnection db = new SqlConnection(_connection);
            int id = Convert.ToInt32(db.ExecuteScalar(sb.ToString(), hotel));
            db.Close();

            return id;
        }

        public void AtualizarCampo(int id, string tabela, string campo, string valor)
        {
            StringBuilder sb = new StringBuilder();

            if (campo == "Cidade") sb.Append($"UPDATE Cidade SET Nome = '{valor}' WHERE Id = {id}");
            else sb.Append($"UPDATE {tabela} SET {campo} = '{valor}' WHERE Id = {id}");

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
            sb.Append(Pacote.INSERT);
            sb.Replace("@Hotel", InserirHotel(pacote.Hotel).ToString());
            sb.Replace("@Passagem", InserirPassagem(pacote.Passagem).ToString());
            sb.Replace("@Cliente", InserirCliente(pacote.Cliente).ToString());
            sb.Append(_identity);

            SqlConnection db = new SqlConnection(_connection);
            db.Open();
            db.Execute(sb.ToString(), pacote);
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
