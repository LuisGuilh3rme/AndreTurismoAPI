namespace API
{
    public class Cliente
    {
        public static readonly string INSERT = "INSERT INTO Cliente (Nome, Telefone, Id_Endereco, Data_Cadastro) VALUES (@Nome, @Telefone, @Endereco, @DataCadastro);";
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public Endereco Endereco { get; set; }
        public DateTime DataCadastro { get; set; }

        public override string ToString()
        {
            return $"\n Nome: {Nome} \n Telefone: {Telefone} \n Endereço: {Endereco} \n Data de cadastro: {DataCadastro}";
        }
    }
}
