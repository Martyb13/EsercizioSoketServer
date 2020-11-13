using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatbotServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Creare il mio socketlistener
            //1) specifico che versione IP
            //2) tipo di socket. Stream.
            //3) protocollo a livello di trasporto

            Socket listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //config: IP dove ascoltare. Possiamo usare l'opzione Any: ascolta da tutte le interfaccie all'interno del mio pc.
            IPAddress ipaddress = IPAddress.Any;

            // config: devo configurare l'EndPoint
            IPEndPoint iPEndPoint = new IPEndPoint(ipaddress, 23000);

            //collego il listener socket all'end point 
            listenerSocket.Bind(iPEndPoint);

            // Mettere in ascolto il server.
            // parametro: il numero massimo di connessioni da mettere in coda.
            listenerSocket.Listen(5);
            Console.WriteLine("Server in ascolto...");
            Console.WriteLine("in attesa di connessione da parte del client...");
            // Istruzione bloccante
            // restituisce una variabile di tipo socket.
            Socket client = listenerSocket.Accept();

            Console.WriteLine("Client IP:" + client.RemoteEndPoint.ToString());

            //mi attrezzo per ricevere un messaggio dal client 
            //siccome è di tippo stream io riceverò un byte array
            //riceverò anche il numero di byte

            byte[] buff = new byte[128];
            int receivedBytes = 0;
            int sendedBytes = 0;
            string receivedString, sendString;
            
            //crea il messaggio
            sendString = "Benvenuto client";

            while (true)
            {
                receivedBytes = client.Receive(buff);
                Console.WriteLine("Numero di byte ricevuti:" + receivedBytes);
                receivedString = Encoding.ASCII.GetString(buff, 0, receivedBytes);
                Console.WriteLine("Stringa ricevuta:" + receivedString);
                if(receivedString != "\r\n")
                {
                    Array.Clear(buff, 0, buff.Length);
                    sendedBytes = 0;
                    
                    if (receivedString.ToUpper() == "QUI")
                    {
                        break;
                    }
                    else if( receivedString.ToLower()=="ciao")
                    {
                        sendString = "ciao";
                    }
                    else if (receivedString.ToLower() == "come stai?")
                    {
                        sendString = "bene";
                    }
                    else if (receivedString.ToLower() == "che fai?")
                    {
                        sendString = "niente";
                    }
                   
                    //lo converto in byte
                    buff = Encoding.ASCII.GetBytes(sendString);

                    //invio al client il messaggio
                    sendedBytes = client.Send(buff);

                    Array.Clear(buff, 0, buff.Length);

                }              
                
            }
            //Termina il programma


        }
    }
}
