using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Text;
using Turismo.Models;

namespace Turismo.Services
{
    internal class TurismoService
    {
        readonly string stringConnection = @"Server=(localdb)\MSSQLLocalDB; Integrated Security=true; AttachDbFileName=C:\Database\17-04\turismo.mdf;";
        readonly SqlConnection SQLConnection;

        public TurismoService()
        {
            SQLConnection = new SqlConnection(stringConnection);
            SQLConnection.Open();
        }

        public int InserirCidade(Cidade cidade)
        {
            string insertString = "INSERT INTO Cidade (Nome) VALUES (@Nome); SELECT CAST(scope_identity() AS INT)";
            SqlCommand insert = new SqlCommand(insertString, SQLConnection);

            insert.Parameters.Add(new SqlParameter("@Nome", cidade.Nome));

            return Convert.ToInt32(insert.ExecuteScalar());
        }

        public int InserirEndereco(Endereco endereco)
        {
            string insertString = "INSERT INTO Endereco (Lodragouro, Numero, Bairro, CEP, Complemento, Id_Cidade, Data_Cadastro) VALUES (@Lgdro, @Nmro, @Bairro, @CEP, @Cpmento, @IdCidade, @Data); SELECT CAST(scope_identity() AS INT)";
            SqlCommand insert = new SqlCommand(@insertString, SQLConnection);

            insert.Parameters.Add(new SqlParameter("@Lgdro", endereco.Logradouro));
            insert.Parameters.Add(new SqlParameter("@Nmro", endereco.Numero));
            insert.Parameters.Add(new SqlParameter("@Bairro", endereco.Bairro));
            insert.Parameters.Add(new SqlParameter("@CEP", endereco.CEP));
            insert.Parameters.Add(new SqlParameter("@Cpmento", endereco.Complemento));
            insert.Parameters.Add(new SqlParameter("@IdCidade", InserirCidade(endereco.Cidade)));
            insert.Parameters.Add(new SqlParameter("@Data", endereco.DataCadastro));

            return Convert.ToInt32(insert.ExecuteScalar());
        }

        public int InserirCliente(Cliente cliente)
        {
            string insertString = "INSERT INTO Cliente (Nome, Telefone, Id_Endereco, Data_Cadastro) VALUES (@Nome, @Telefone, @IdEndereco, @Cadastro); SELECT CAST(scope_identity() AS INT)";
            SqlCommand insert = new SqlCommand(@insertString, SQLConnection);

            insert.Parameters.Add(new SqlParameter("@Nome", cliente.Nome));
            insert.Parameters.Add(new SqlParameter("@Telefone", cliente.Telefone));
            insert.Parameters.Add(new SqlParameter("@IdEndereco", InserirEndereco(cliente.Endereco)));
            insert.Parameters.Add(new SqlParameter("@Cadastro", cliente.DataCadastro));

            return Convert.ToInt32(insert.ExecuteScalar());
        }

        public int InserirPassagem(Passagem passagem)
        {
            string insertString = "INSERT INTO Passagem (Id_Origem, Id_Destino, Id_Cliente, Data, Valor) VALUES (@Org, @Dest, @IdCliente, @Data, @Valor); SELECT CAST(scope_identity() AS INT)";
            SqlCommand insert = new SqlCommand(@insertString, SQLConnection);

            insert.Parameters.Add(new SqlParameter("@Org", InserirEndereco(passagem.Origem)));
            insert.Parameters.Add(new SqlParameter("@Dest", InserirEndereco(passagem.Destino)));
            insert.Parameters.Add(new SqlParameter("@IdCliente", InserirCliente(passagem.Cliente)));
            insert.Parameters.Add(new SqlParameter("@Data", passagem.Data));
            insert.Parameters.Add(new SqlParameter("@Valor", passagem.Valor));

            return Convert.ToInt32(insert.ExecuteScalar());
        }

        public int InserirHotel(Hotel hotel)
        {
            string insertString = "INSERT INTO Hotel (Nome, Id_Endereco, Data_Cadastro, Valor) VALUES (@Nome, @IdEndereco, @Cadastro, @Valor); SELECT CAST(scope_identity() AS INT)";
            SqlCommand insert = new SqlCommand(@insertString, SQLConnection);

            insert.Parameters.Add(new SqlParameter("@Nome", hotel.Nome));
            insert.Parameters.Add(new SqlParameter("@IdEndereco", InserirEndereco(hotel.Endereco)));
            insert.Parameters.Add(new SqlParameter("@Cadastro", hotel.DataCadastro));
            insert.Parameters.Add(new SqlParameter("@Valor", hotel.Valor));

            return Convert.ToInt32(insert.ExecuteScalar());
        }

        public int Inserir(Pacote pacote)
        {
            string insertString = "INSERT INTO Pacote (Id_Hotel, Id_Passagem, Data_Cadastro, Id_Cliente, Valor) VALUES (@IdHotel, @IdPassagem, @Cadastro, @IdCliente, @Valor); SELECT CAST(scope_identity() AS INT)";
            SqlCommand insert = new SqlCommand(@insertString, SQLConnection);

            insert.Parameters.Add(new SqlParameter("@IdHotel", InserirHotel(pacote.Hotel)));
            insert.Parameters.Add(new SqlParameter("@IdPassagem", InserirPassagem(pacote.Passagem)));
            insert.Parameters.Add(new SqlParameter("@Cadastro", pacote.DataCadastro));
            insert.Parameters.Add(new SqlParameter("@IdCliente", InserirCliente(pacote.Cliente)));
            insert.Parameters.Add(new SqlParameter("@Valor", pacote.Valor));

            return Convert.ToInt32(insert.ExecuteScalar());
        }

        /*public List<Pacote> ListarPacotes()
        {
            List<Pacote> pacotes = new List<Pacote>();

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT a.Id, a.Name, a.PassagersNumber, a.Description, e.Description AS \"Engine_Description\" FROM Airplane a, Engine e WHERE a.Id_Engine = e.Id");

            SqlCommand select = new SqlCommand(sb.ToString(), SQLConnection);
            SqlDataReader dr = select.ExecuteReader();

            while (dr.Read())
            {
                Pacote airplane = new Pacote();

                airplane.Id = Convert.ToInt32(dr["Id"]);
                airplane.Name = Convert.ToString(dr["Name"])!;
                airplane.PassagersNumber = Convert.ToInt32(dr["PassagersNumber"]);
                airplane.Description = Convert.ToString(dr["Description"])!;
                airplane.Engine = new Engine
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    Description = Convert.ToString(dr["Engine_Description"])!
                };
                airplanes.Add(airplane);
            }

            return airplanes;
        }*/

        public Hotel RetornarHotel(int id)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Id, Nome, Id_Endereco, Data_Cadastro, Valor FROM Hotel WHERE Id = " + id);

            SqlCommand select = new SqlCommand(sb.ToString(), SQLConnection);
            SqlDataReader dr = select.ExecuteReader();

            Hotel hotel = new Hotel();

            if (dr.Read())
            {
                hotel.Id = Convert.ToInt32(dr["Id"]);
                hotel.Nome = Convert.ToString(dr["Nome"]);
                hotel.Endereco = RetornarEndereco(Convert.ToInt32(dr["Id_Endereco"]));
                hotel.DataCadastro = DateTime.Parse(dr["Data_Cadastro"].ToString());
                hotel.Valor = Convert.ToDecimal(dr["Valor"]);
            }

            return hotel;
        }

        public Endereco RetornarEndereco (int id)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Id, Logradouro, Numero, Bairro, CEP, Complemento, Id_Cidade, Data_Cadastro FROM Endereco WHERE Id = " + id);

            SqlCommand select = new SqlCommand(sb.ToString(), SQLConnection);
            SqlDataReader dr = select.ExecuteReader();

            Endereco endereco = new Endereco();

            if (dr.Read())
            {
                endereco.Id = Convert.ToInt32(dr["Id"]);
                endereco.Logradouro = Convert.ToString(dr["Logradouro"]);
                endereco.Bairro = Convert.ToString(dr["Bairro"]);
                endereco.CEP = Convert.ToString(dr["CEP"]);
                endereco.Complemento = Convert.ToString(dr["Complemento"]);
                endereco.Cidade = RetornarCidade(Convert.ToInt32(dr["Id_Cidade"]));
                endereco.DataCadastro = DateTime.Parse(dr["Data_Cadastro"].ToString());
            }

            return endereco;
        }

        public Cidade RetornarCidade (int id)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Id, Nome FROM Cidade WHERE Id = " + id);

            SqlCommand select = new SqlCommand(sb.ToString(), SQLConnection);
            SqlDataReader dr = select.ExecuteReader();

            Cidade cidade = new Cidade();

            if (dr.Read())
            {
                cidade.Id = Convert.ToInt32(dr["Id"]);
                cidade.Nome = Convert.ToString(dr["Nome"]);
            }

            return cidade;
        }

        public Cliente RetornarCliente(int id)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Id, Nome, Telefone, Endereco, Data_Cadastro FROM Cliente WHERE Id = " + id);

            SqlCommand select = new SqlCommand(sb.ToString(), SQLConnection);
            SqlDataReader dr = select.ExecuteReader();

            Cliente cliente = new Cliente();

            if (dr.Read())
            {
                cliente.Id = Convert.ToInt32(dr["Id"]);
                cliente.Nome = Convert.ToString(dr["Nome"]);
                cliente.Telefone = Convert.ToString(dr["Telefone"]);
                cliente.Endereco = RetornarEndereco(Convert.ToInt32(dr["Id_Endereco"]));
                cliente.DataCadastro = DateTime.Parse(dr["Data_Cadastro"].ToString());
            }

            return cliente;
        }

        public Passagem RetornarPassagem (int id)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Id, Id_Origem, Id_Destino, Id_Cliente, Data, Valor FROM Passagem WHERE Id = " + id);

            SqlCommand select = new SqlCommand(sb.ToString(), SQLConnection);
            SqlDataReader dr = select.ExecuteReader();

            Passagem passagem = new Passagem();

            if (dr.Read())
            {
                passagem.Id = Convert.ToInt32(dr["Id"]);
                passagem.Origem = RetornarEndereco(Convert.ToInt32(dr["Id_Origem"]));
                passagem.Destino = RetornarEndereco(Convert.ToInt32(dr["Id_Destino"]));
                passagem.Cliente = RetornarCliente(Convert.ToInt32(dr["Id_Cliente"]));
                passagem.Data = DateTime.Parse(dr["Data"].ToString());
                passagem.Valor = Convert.ToDecimal(dr["Valor"]);
            }

            return passagem;
        }
    }
}
