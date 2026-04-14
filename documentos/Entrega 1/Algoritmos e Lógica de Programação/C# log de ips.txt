using System;
using System.Collections.Generic;
using System.IO;

class VerificacaoIP
{
    static void Main()
    {
        // Lista de IPs autorizados (simulação)
        List<string> ipsAutorizados = new List<string>()
        {
            "192.168.0.1",
            "10.0.0.5"
        };

        Console.Write("Digite o IP de origem: ");
        string ipEntrada = Console.ReadLine();

        bool autorizado = ipsAutorizados.Contains(ipEntrada);

        if (autorizado)
        {
            Console.WriteLine("Acesso AUTORIZADO");
            RegistrarLog(ipEntrada, "AUTORIZADO");
        }
        else
        {
            Console.WriteLine("Acesso BLOQUEADO");
            RegistrarLog(ipEntrada, "BLOQUEADO");
        }
    }

    static void RegistrarLog(string ip, string status)
    {
        string caminho = "log.txt";
        string registro = $"{DateTime.Now} - IP: {ip} - Status: {status}";

        File.AppendAllText(caminho, registro + Environment.NewLine);
    }
}
