using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Text;
using System.Windows.Markup;
using Turismo.Controllers;
using Turismo.Models;
using Turismo.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        int opt = Menu();

        while (opt != 4)
        {
            switch (opt)
            {
                case 1: CadastrarPacote(); break;
                case 2:
                    int count = 0;
                    new TurismoController().FindAll().ForEach(pacote =>
                    {
                        Console.WriteLine("*****************");
                        Console.WriteLine("PACOTE NÚMERO: " + ++count);
                        Console.WriteLine(pacote.ToString());
                        Console.WriteLine();
                    });
                    break;
                case 3: AtualizarRegistros(); break;
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
        Console.WriteLine("4 - Sair");
        int opt = int.Parse(Console.ReadLine());
        return opt;
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

        ConsoleColor aux = Console.ForegroundColor;
        for (int i = 0; i < pacotes.Count; i++)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("**REGISTRO {0}**", i + 1);
            Console.ForegroundColor = aux;

            Console.WriteLine(pacotes[i].ToString());
            Console.WriteLine();
        }
        Console.Write("Escolha o registro: ");
        int index = int.Parse(Console.ReadLine()) - 1;
        verificador = EncontrarIndex(pacotes.Count, index);
        if (!verificador) { return; }

        Console.Clear();
        Console.WriteLine(" Registro escolhido: ");
        Console.WriteLine(pacotes[index].ToString());

        string[] tabelaPacote = { "Hotel", "Passagem", "DataCadastro", "Valor", "Cliente" };
        ImprimirPropriedades(tabelaPacote);
        Console.Write(" Escolha o index: ");
        int indexPacote = int.Parse(Console.ReadLine()) - 1;
        verificador = EncontrarIndex(tabelaPacote.Length, indexPacote);
        if (!verificador) { return; }

        switch (indexPacote)
        {
            case 0: if (!AtualizarHotel(pacotes[index].Hotel)) return; break;
        }
    }

    private static bool AtualizarHotel(Hotel hotel)
    {
        Console.Clear();
        Console.WriteLine(hotel.ToString());
        string[] tabela = { "Nome", "Endereco", "DataCadastro", "Valor" };

        ImprimirPropriedades(tabela);
        Console.Write("Escolha o index: ");
        int index = int.Parse(Console.ReadLine()) - 1;

        bool verificador = EncontrarIndex(tabela.Length, index);
        if (!verificador) return false;

        Console.Clear();
        string atualizacao = String.Empty;
        switch (index)
        {
            case 0:
                Console.WriteLine("Atual: " + hotel.Nome);
                Console.WriteLine("Novo nome: ");
                atualizacao = Console.ReadLine();
                new TurismoService().AtualizarHotel(hotel.Id, tabela[index], atualizacao);
                break;

            case 3:
                Console.WriteLine("Atual: " + hotel.Valor);
                Console.WriteLine("Atualizar Valor: ");
                atualizacao = DateTime.Parse(Console.ReadLine()).ToShortDateString();
                new TurismoService().AtualizarHotel(hotel.Id, tabela[index], atualizacao);
                break;
        }

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
            Console.WriteLine("Index inválido");
            Console.WriteLine("ENTER para voltar");
            Console.ReadLine();
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