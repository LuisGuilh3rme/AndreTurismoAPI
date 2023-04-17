using System.Data.SqlClient;
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

        public int InserirCidade (Cidade cidade)
        {
            string insertString = "INSERT INTO Cidade (Nome) VALUES (@Nome); SELECT CAST(scope_identity() AS INT)";
            SqlCommand insert = new SqlCommand(insertString, SQLConnection);

            insert.Parameters.Add(new SqlParameter("@Nome", cidade.Nome));

            return Convert.ToInt32(insert.ExecuteScalar());
        }

        public int InserirEndereco (Endereco endereco)
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

        public int InserirCliente (Cliente cliente)
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
            string insertString = "INSERT INTO Passagem (Origem, Destino, Id_Cliente, Data, Valor) VALUES (@Org, @Dest, @IdCliente, @Data, @Valor); SELECT CAST(scope_identity() AS INT)";
            SqlCommand insert = new SqlCommand(@insertString, SQLConnection);

            insert.Parameters.Add(new SqlParameter("@Org", InserirEndereco(passagem.Origem)));
            insert.Parameters.Add(new SqlParameter("@Dest", InserirEndereco(passagem.Destino)));
            insert.Parameters.Add(new SqlParameter("@IdCliente", InserirCliente(passagem.Cliente)));
            insert.Parameters.Add(new SqlParameter("@Data", passagem.Data));
            insert.Parameters.Add(new SqlParameter("@Valor", passagem.Valor));

            return Convert.ToInt32(insert.ExecuteScalar());
        }
    }
}
