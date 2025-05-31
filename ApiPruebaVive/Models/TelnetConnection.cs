using System.Net.Sockets;
using System.Text;

namespace ApiPruebaVive.Models
{
    enum Verbs
    {
        WILL = 251,
        WONT = 252,
        DO = 253,
        DONT = 254,
        IAC = 255
    }

    enum Options
    {
        SGA = 3
    }

    public class TelnetConnection
    {



        TcpClient tcpSocket;

        public int TimeOutMs = 300;

        public TelnetConnection(string Hostname, int Port)
        {

            try
            {
                tcpSocket = new TcpClient(Hostname, Port);
                if (tcpSocket.Connected)
                {
                    System.Threading.Thread.Sleep(1000);
                }
                else
                {
                    throw new Exception("No se pudo establecer una conexion con la OLT, por favor intentelo Nuevamente Error 400");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo establecer una conexion con la OLT, por favor intentelo Nuevamente " + ex.Message);
            }
        }


        public void desconectar()
        {
            tcpSocket.Close();
        }
        public string Login(string Username, string Password, int LoginTimeOutMs)
        {
            int oldTimeOutMs = TimeOutMs;
            TimeOutMs = LoginTimeOutMs;
            string s = Read();
            if (!s.TrimEnd().EndsWith(":"))
                desconectar();
            WriteLine(Username);

            s += Read();
            if (!s.TrimEnd().EndsWith(":"))
                desconectar();
            WriteLine(Password);

            s += Read();
            TimeOutMs = oldTimeOutMs;
            return s;
        }

        public void WriteLine(string cmd)
        {
            Write(cmd + "\n");
        }

        public void Write(string cmd)
        {
            try
            {
                if (!tcpSocket.Connected) return;
                byte[] buf = System.Text.ASCIIEncoding.ASCII.GetBytes(cmd.Replace("\0xFF", "\0xFF\0xFF"));
                tcpSocket.GetStream().Write(buf, 0, buf.Length);
            }
            catch (Exception)
            {
                throw new Exception("Se ha perdido la conexion con la OLT, Reintentalo nuevamente");
            }

        }

        public string Read(int timeoutMs = 3000)
        {
            if (!tcpSocket.Connected)
                return null;

            var sb = new StringBuilder();
            var start = DateTime.Now;

            while ((DateTime.Now - start).TotalMilliseconds < timeoutMs)
            {
                if (tcpSocket.Available > 0)
                {
                    ParseTelnet(sb);
                    start = DateTime.Now; // Reinicia el timer porque recibimos datos
                }
                else
                {
                    Thread.Sleep(50);
                }
            }

            return sb.ToString();
        }

        public string ReadUntilPrompt(string prompt = "ZXAN#", int timeoutMs = 8000)
        {
            var buffer = new StringBuilder();
            var start = DateTime.Now;

            while ((DateTime.Now - start).TotalMilliseconds < timeoutMs)
            {
                string chunk = Read(500);  // Llama a tu método Read con timeout interno
                if (!string.IsNullOrEmpty(chunk))
                {
                    buffer.Append(chunk);

                    if (buffer.ToString().Contains(prompt))
                    {
                        break;
                    }
                }
                else
                {
                    Thread.Sleep(50); // Espera un poco antes de seguir leyendo
                }
            }

            return buffer.ToString();
        }

        public bool IsConnected
        {
            get { return tcpSocket.Connected; }
        }

        void ParseTelnet(StringBuilder sb)
        {
            while (tcpSocket.Available > 0)
            {
                int input = tcpSocket.GetStream().ReadByte();
                switch (input)
                {
                    case -1:
                        break;
                    case (int)Verbs.IAC:
                        // interpret as command
                        int inputverb = tcpSocket.GetStream().ReadByte();
                        if (inputverb == -1) break;
                        switch (inputverb)
                        {
                            case (int)Verbs.IAC:
                                //literal IAC = 255 escaped, so append char 255 to string
                                sb.Append(inputverb);
                                break;
                            case (int)Verbs.DO:
                            case (int)Verbs.DONT:
                            case (int)Verbs.WILL:
                            case (int)Verbs.WONT:
                                // reply to all commands with "WONT", unless it is SGA (suppres go ahead)
                                int inputoption = tcpSocket.GetStream().ReadByte();
                                if (inputoption == -1) break;
                                tcpSocket.GetStream().WriteByte((byte)Verbs.IAC);
                                if (inputoption == (int)Options.SGA)
                                    tcpSocket.GetStream().WriteByte(inputverb == (int)Verbs.DO ? (byte)Verbs.WILL : (byte)Verbs.DO);
                                else
                                    tcpSocket.GetStream().WriteByte(inputverb == (int)Verbs.DO ? (byte)Verbs.WONT : (byte)Verbs.DONT);
                                tcpSocket.GetStream().WriteByte((byte)inputoption);
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        sb.Append((char)input);
                        break;
                }
            }
        }
    }

}

