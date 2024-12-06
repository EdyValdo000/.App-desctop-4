using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace App_desctop_4.Conection
{
    public class ethernet
    {
        private TcpClient tcpCliente;
        private IPAddress enderecoIP;
        private StreamWriter stwEnviar;
        private StreamReader strReceber;
        private string receber;

        // Função para conectar ao servidor (Arduino)
        public void conected(string EnderecoIP, Int16 Porta)
        {
            try
            {
                enderecoIP = IPAddress.Parse(EnderecoIP);
                tcpCliente = new TcpClient();
                tcpCliente.Connect(enderecoIP, Porta);

                stwEnviar = new StreamWriter(tcpCliente.GetStream());
                strReceber = new StreamReader(tcpCliente.GetStream());

                stwEnviar.Write('s'); // Envia uma mensagem inicial de conexão
                stwEnviar.Flush();

                Console.WriteLine("Conectado com sucesso ao Arduino");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro na conexão: {ex.Message}");
            }
        }

        // Função para desconectar
        public void desconected()
        {
            try
            {
                if (stwEnviar != null) stwEnviar.Close();
                if (strReceber != null) strReceber.Close();
                if (tcpCliente != null) tcpCliente.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao desconectar: {ex.Message}");
            }
        }

        // Função para enviar dados
        public void send(string sendString)
        {
            try
            {
                if (tcpCliente != null && tcpCliente.Connected)
                {
                    stwEnviar.Write(sendString);
                    stwEnviar.Flush();
                }
                else
                {
                    Console.WriteLine("Erro: Não conectado ao Arduino.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar dados: {ex.Message}");
            }
        }

        // Função para ler dados recebidos
        public string read()
        {
            try
            {
                if (tcpCliente != null && tcpCliente.Connected)
                {
                    // Verifica se há dados disponíveis para leitura antes de chamar Read()
                    if (tcpCliente.GetStream().DataAvailable)
                    {
                        // Usando ReadLine() ou ReadToEnd(), dependendo do que o Arduino enviar
                        StringBuilder recebidos = new StringBuilder();
                        while (tcpCliente.GetStream().DataAvailable)
                        {
                            char c = (char)strReceber.Read(); // Lê um caractere de cada vez
                            recebidos.Append(c); // Adiciona o caractere ao StringBuilder
                        }
                        return recebidos.ToString(); // Retorna os dados recebidos
                    }
                    else
                    {
                        return null; // Sem dados disponíveis
                    }
                }
                else
                {
                    Console.WriteLine("Erro: Não conectado ao Arduino.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao ler dados: {ex.Message}");
                return null;
            }
        }


        // Verificação do estado de conexão
        public bool isConected()
        {
            return tcpCliente != null && tcpCliente.Connected;
        }
    }
}
