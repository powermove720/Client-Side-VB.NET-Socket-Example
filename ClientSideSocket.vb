Imports System.Net
Imports System.Net.Sockets

Module SocketClient
    Sub Main()
        Dim host As String = "127.0.0.1"
        Dim port As Integer = 12345
        
        Try
            Dim client As TcpClient = New TcpClient(host, port)
            Dim stream As NetworkStream = client.GetStream()
            
            Dim bytesRead As Integer = stream.Read(bytes, 0, bytes.Length)
            
            If bytesRead > 0 Then
                Dim message As String = Encoding.ASCII.GetString(bytes, 0, bytesRead)
                Console.WriteLine("Received message: " & message)
            End If
            
            Dim response As String = "Hello from the client!"
            Byte[] responseBytes = Encoding.ASCII.GetBytes(response)
            stream.Write(responseBytes, 0, responseBytes.Length)
            
            Do While True
                bytesRead = stream.Read(bytes, 0, bytes.Length)
                
                If bytesRead > 0 Then
                    Dim message As String = Encoding.ASCII.GetString(bytes, 0, bytesRead)
                    Console.WriteLine("Received response: " & message)
                End If
                
                If message.ToLower().Contains("quit") Then
                    stream.Write(bytes, 0, bytesRead)
                    Exit Do
                End If
            Loop
        Catch ex As SocketException
            Console.WriteLine("Socket error: " & ex.Message)
            If ex.SocketErrorCode = SocketError.ConnectionReset Then
                Console.WriteLine("Server disconnected.")
            ElseIf ex.SocketErrorCode = SocketError.HostUnreachable Then
                Console.WriteLine("Server unreachable.")
            End If
        Catch ex As IOException

            Console.WriteLine("IO exception: " & ex.Message)
        Finally
            client.Close()
        End Try
        
        Console.ReadLine()
    End Sub
End Module
