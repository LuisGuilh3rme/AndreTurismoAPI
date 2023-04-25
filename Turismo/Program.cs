using System.Text;
using Turismo.Controllers;
using Turismo.Models;
using Turismo.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        int opt = Menu();

        while (opt != 5)
        {
            switch (opt)
            {
                case 1: CadastrarPacote(); break;
                case 2:
                    List<Pacote> pacotes = new TurismoController().FindAll();
                    ImprimirPacotes(pacotes);
                    break;

                case 3: AtualizarRegistros(); break;
                case 4: RemoverPacote(); break;
            }

            Console.WriteLine("**ENTER para continuar**");
            Console.ReadLine();
            opt = Menu();
        }
    }

    private static int Menu()
    {
        Console.Clear();
        Console.WriteLine("1 - Adicionar pacotes");
        Console.WriteLine("2 - Listar pacotes");
        Console.WriteLine("3 - Atualizar tabelas");
        Console.WriteLine("4 - Apagar pacotes");
        Console.WriteLine("5 - Sair");
        int opt = int.Parse(Console.ReadLine());
        return opt;
    }

    private static void ImprimirPacotes(List<Pacote> pacotes)
    {
        ConsoleColor aux = Console.ForegroundColor;
        for (int i = 0; i < pacotes.Count; i++)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("**REGISTRO {0}**", i + 1);
            Console.ForegroundColor = aux;

            Console.WriteLine(pacotes[i].ToString());
            Console.WriteLine();
        }
    }

    private static void CadastrarPacote()
    {
        Pacote pacote = new Pacote
        {
            Hotel = new Hotel
            {
                Nome = NomeHotelRandom(),
                Endereco = new Endereco
                {
                    Logradouro = NomeLogradouroRandom(),
                    Numero = new Random().Next(1000),
                    Bairro = NomeBairroRandom(),
                    CEP = CEPRandom(),
                    Complemento = "null",
                    Cidade = new Cidade { Nome = NomeCidadeRandom() },
                    DataCadastro = DateTime.Now
                },
                DataCadastro = DateTime.Now,
                Valor = 1.99m
            },
            Passagem = new Passagem
            {
                Origem = new Endereco
                {
                    Logradouro = NomeLogradouroRandom(),
                    Numero = new Random().Next(1000),
                    Bairro = NomeBairroRandom(),
                    CEP = CEPRandom(),
                    Complemento = "null",
                    Cidade = new Cidade { Nome = NomeCidadeRandom() },
                    DataCadastro = DateTime.Now
                },
                Destino = new Endereco
                {
                    Logradouro = NomeLogradouroRandom(),
                    Numero = new Random().Next(1000),
                    Bairro = NomeBairroRandom(),
                    CEP = CEPRandom(),
                    Complemento = "Sala 7",
                    Cidade = new Cidade { Nome = NomeCidadeRandom() },
                    DataCadastro = DateTime.Now
                },
                Cliente = new Cliente
                {
                    Nome = NomeClienteRandom(),
                    Telefone = TelefoneRandom(),
                    Endereco = new Endereco
                    {
                        Logradouro = NomeLogradouroRandom(),
                        Numero = new Random().Next(1000),
                        Bairro = NomeBairroRandom(),
                        CEP = CEPRandom(),
                        Complemento = "Sala 7",
                        Cidade = new Cidade { Nome = NomeCidadeRandom() },
                        DataCadastro = DateTime.Now
                    },
                    DataCadastro = DateTime.Now
                },
                Data = DateTime.Now,
                Valor = 100.99m
            },
            DataCadastro = DateTime.Now,
            Valor = 101.99m,
            Cliente = new Cliente
            {
                Nome = NomeClienteRandom(),
                Telefone = TelefoneRandom(),
                Endereco = new Endereco
                {
                    Logradouro = NomeLogradouroRandom(),
                    Numero = new Random().Next(1000),
                    Bairro = NomeBairroRandom(),
                    CEP = CEPRandom(),
                    Complemento = "null",
                    Cidade = new Cidade { Nome = NomeCidadeRandom() },
                    DataCadastro = DateTime.Now
                },
                DataCadastro = DateTime.Now
            }
        };
        new TurismoController().Insert(pacote);
    }

    private static void AtualizarRegistros()
    {
        Console.Clear();
        List<Pacote> pacotes = new TurismoController().FindAll();
        bool verificador = true;

        ImprimirPacotes(pacotes);
        Console.Write("Escolha o registro: ");
        int index = int.Parse(Console.ReadLine()) - 1;
        verificador = EncontrarIndex(pacotes.Count, index);
        if (!verificador) { return; }

        AtualizarPacote(pacotes[index]);
    }

    private static void RemoverPacote()
    {
        Console.Clear();
        List<Pacote> pacotes = new TurismoController().FindAll();
        bool verificador = true;

        ImprimirPacotes(pacotes);
        Console.Write("Escolha o registro: ");
        int index = int.Parse(Console.ReadLine()) - 1;
        verificador = EncontrarIndex(pacotes.Count, index);
        if (!verificador) { return; }

        new TurismoService().RemoverPacote(pacotes[index].Id);
    }

    private static bool AtualizarPacote(Pacote pacote)
    {
        Console.Clear();
        Console.WriteLine(pacote.ToString());
        string[] tabela = { "Hotel", "Passagem", "DataCadastro", "Valor", "Cliente" };

        ImprimirPropriedades(tabela);
        Console.Write("Escolha o index: ");
        int index = int.Parse(Console.ReadLine()) - 1;

        bool verificador = EncontrarIndex(tabela.Length, index);
        bool ehString = true;
        if (!verificador) return false;

        Console.Clear();
        switch (index)
        {
            case 0:
                AtualizarHotel(pacote.Hotel);
                ehString = false;
                break;

            case 1:
                AtualizarPassagem(pacote.Passagem);
                ehString = false;
                break;

            case 2:
                Console.WriteLine("Atual: " + pacote.DataCadastro);
                Console.WriteLine("Atualizar data de cadastro (mes/dia/ano): ");
                break;

            case 3:
                Console.WriteLine("Atual: " + pacote.Valor);
                Console.WriteLine("Atualizar data de cadastro (mes/dia/ano): ");
                break;

            case 4:
                AtualizarCliente(pacote.Cliente);
                ehString = false;
                break;
        }

        if (ehString)
        {
            string atualizacao = Console.ReadLine();
            new TurismoService().AtualizarCampo(pacote.Id, "Pacote", tabela[index], atualizacao);
        }
        return true;
    }

    private static bool AtualizarCliente(Cliente cliente)
    {
        Console.Clear();
        Console.WriteLine(cliente.ToString());
        string[] tabela = { "Nome", "Telefone", "Endereco", "Data_Cadastro" };

        ImprimirPropriedades(tabela);
        Console.Write("Escolha o index: ");
        int index = int.Parse(Console.ReadLine()) - 1;

        bool verificador = EncontrarIndex(tabela.Length, index);
        bool ehString = true;
        if (!verificador) return false;

        Console.Clear();
        switch (index)
        {
            case 0:
                Console.WriteLine("Atual: " + cliente.Nome);
                Console.WriteLine("Atualizar nome: ");
                break;

            case 1:
                Console.WriteLine("Atual: " + cliente.Telefone);
                Console.WriteLine("Atualizar telefone: ");
                break;

            case 2:
                AtualizarEndereco(cliente.Endereco);
                ehString = false;
                break;

            case 3:
                Console.WriteLine("Atual: " + cliente.DataCadastro);
                Console.WriteLine("Atualizar data de cadastro (mes/dia/ano): ");
                break;
        }

        if (ehString)
        {
            string atualizacao = Console.ReadLine();
            new TurismoService().AtualizarCampo(cliente.Id, "Cliente", tabela[index], atualizacao);
        }
        return true;
    }

    private static bool AtualizarPassagem(Passagem passagem)
    {
        Console.Clear();
        Console.WriteLine(passagem.ToString());
        string[] tabela = { "Endereco_Origem", "Endereco_Destino", "Cliente", "Data", "Valor" };

        ImprimirPropriedades(tabela);
        Console.Write("Escolha o index: ");
        int index = int.Parse(Console.ReadLine()) - 1;

        bool verificador = EncontrarIndex(tabela.Length, index);
        bool ehString = true;
        if (!verificador) return false;

        Console.Clear();
        switch (index)
        {
            case 0:
                AtualizarEndereco(passagem.Origem);
                ehString = false;
                break;

            case 1:
                AtualizarEndereco(passagem.Origem);
                ehString = false;
                break;

            case 2:
                AtualizarCliente(passagem.Cliente);
                ehString = false;
                break;

            case 3:
                Console.WriteLine("Atual: " + passagem.Data);
                Console.WriteLine("Atualizar data de viagem (mes/dia/ano): ");
                break;

            case 4:
                Console.WriteLine("Atual: " + passagem.Valor);
                Console.WriteLine("Atualizar valor: ");
                break;

        }

        if (ehString)
        {
            string atualizacao = Console.ReadLine();
            new TurismoService().AtualizarCampo(passagem.Id, "Passagem", tabela[index], atualizacao);
        }
        return true;
    }

    private static bool AtualizarHotel(Hotel hotel)
    {
        Console.Clear();
        Console.WriteLine(hotel.ToString());
        string[] tabela = { "Nome", "Endereco", "Data_Cadastro", "Valor" };

        ImprimirPropriedades(tabela);
        Console.Write("Escolha o index: ");
        int index = int.Parse(Console.ReadLine()) - 1;

        bool verificador = EncontrarIndex(tabela.Length, index);
        bool ehString = true;
        if (!verificador) return false;

        Console.Clear();
        switch (index)
        {
            case 0:
                Console.WriteLine("Atual: " + hotel.Nome);
                Console.WriteLine("Atualizar nome: ");
                break;

            case 1:
                AtualizarEndereco(hotel.Endereco);
                ehString = false;
                break;

            case 2:
                Console.WriteLine("Atual: " + hotel.DataCadastro);
                Console.WriteLine("Atualizar data de cadastro (mes/dia/ano): ");
                break;

            case 3:
                Console.WriteLine("Atual: " + hotel.Valor);
                Console.WriteLine("Atualizar Valor: ");
                break;

        }

        if (ehString)
        {
            string atualizacao = Console.ReadLine();
            new TurismoService().AtualizarCampo(hotel.Id, "Hotel", tabela[index], atualizacao);
        }
        return true;
    }

    private static bool AtualizarEndereco(Endereco endereco)
    {
        Console.Clear();
        Console.WriteLine(endereco.ToString());
        string[] tabela = { "Logradouro", "Numero", "Bairro", "CEP", "Complemento", "Cidade", "Data_Cadastro" };

        ImprimirPropriedades(tabela);
        Console.Write("Escolha o index: ");
        int index = int.Parse(Console.ReadLine()) - 1;

        bool verificador = EncontrarIndex(tabela.Length, index);
        bool ehString = true;
        if (!verificador) return false;

        Console.Clear();
        switch (index)
        {
            case 0:
                Console.WriteLine("Atual: " + endereco.Logradouro);
                Console.WriteLine("Atualizar rua: ");
                break;

            case 1:
                Console.WriteLine("Atual: " + endereco.Numero);
                Console.WriteLine("Atualizar numero da casa: ");
                break;

            case 2:
                Console.WriteLine("Atual: " + endereco.Bairro);
                Console.WriteLine("Atualizar Bairro: ");
                break;

            case 3:
                Console.WriteLine("Atual: " + endereco.CEP);
                Console.WriteLine("Atualizar CEP: ");
                break;

            case 4:
                Console.WriteLine("Atual: " + endereco.Complemento);
                Console.WriteLine("Atualizar Complemento: ");
                break;

            case 5:
                Console.WriteLine("Atual: " + endereco.Cidade.Nome);
                Console.WriteLine("Atualizar Nome cidade: ");
                break;

            case 6:
                Console.WriteLine("Atual: " + endereco.DataCadastro);
                Console.WriteLine("Atualizar Data de cadastro(mes/dia/ano): ");
                break;
        }

        string atualizacao = Console.ReadLine();
        new TurismoService().AtualizarCampo(endereco.Id, "Endereco", tabela[index], atualizacao);
        return true;
    }

    private static void ImprimirPropriedades(string[] tabela)
    {
        Console.WriteLine();
        for (int i = 0; i < tabela.Length; i++)
        {
            Console.WriteLine(" {0} - Atualizar {1}", i + 1, tabela[i]);
        }
        Console.WriteLine();
    }

    private static bool EncontrarIndex(int tam, int index)
    {
        if (index < 0 || index > tam - 1)
        {
            ConsoleColor aux = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Index inválido");
            Console.ForegroundColor = aux;
            return false;
        }
        return true;
    }

    private static string NomeHotelRandom()
    {
        switch (new Random().Next(10))
        {
            case 1: return "Hotel five STARS";
            case 2: return "Hotelaria";
            case 3: return "Hotel da esquina";
            case 4: return "Hotel unico";
            case 5: return "Hotel QI Negativo";
            case 6: return "Hotel Five By Five";
            case 7: return "Hosstel";
            case 8: return "Hotel Fim de mundo";
            case 9: return "Hotel top";
            case 0: return "Hotel Lorem Ipsum";
        }
        return "null";
    }

    private static string NomeClienteRandom()
    {
        switch (new Random().Next(10))
        {
            case 1: return "Guilherme";
            case 2: return "Annaconda";
            case 3: return "Paola Dhentro";
            case 4: return "Thiago";
            case 5: return "Amanda";
            case 6: return "Tia May";
            case 7: return "Kim Catupiry";
            case 8: return "Renato Russo";
            case 9: return "Goku";
            case 0: return "John Doe";
        }
        return "null";
    }

    private static string NomeBairroRandom()
    {
        switch (new Random().Next(10))
        {
            case 1: return "Jardim das rosas";
            case 2: return "Jardim dos lirios";
            case 3: return "Jardim da jardinagem";
            case 4: return "Jardim 1 antes do jardim 2";
            case 5: return "Anhanguera";
            case 6: return "Residencial Paraiso";
            case 7: return "Bairro do Victoria Business";
            case 8: return "Casa verde";
            case 9: return "Piccolo";
            case 0: return "Jardim Dolor sit amet";
        }
        return "null";
    }

    private static string NomeLogradouroRandom()
    {
        switch (new Random().Next(10))
        {
            case 1: return "rua five STARS";
            case 2: return "rua da esquina";
            case 3: return "rua da verdinha";
            case 4: return "rua unica";
            case 5: return "rua QI Negativo";
            case 6: return "rua Five By Five";
            case 7: return "rua bonita";
            case 8: return "rua Fim de mundo";
            case 9: return "rua top";
            case 0: return "rua Lorem Ipsum";
        }
        return "null";
    }

    private static string NomeCidadeRandom()
    {
        switch (new Random().Next(10))
        {
            case 1: return "Jaboticabal";
            case 2: return "Ibitinga";
            case 3: return "Taquaritinga";
            case 4: return "Matão";
            case 5: return "Paris";
            case 6: return "Santos";
            case 7: return "Araraquara";
            case 8: return "Minas Gerais";
            case 9: return "SeteAlem";
            case 0: return "Lorem Ipsum";
        }
        return "null";
    }

    private static string CEPRandom()
    {
        Random rdm = new Random();

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < 8; i++)
        {
            sb.Append(rdm.Next(10).ToString());
        }

        return sb.ToString();
    }

    private static string TelefoneRandom()
    {
        Random rdm = new Random();

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < 8; i++)
        {
            if (i == 3) sb.Append("-");
            sb.Append(rdm.Next(10).ToString());
        }

        return sb.ToString();
    }
}