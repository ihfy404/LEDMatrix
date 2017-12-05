import socket
import sys

param = ["0","0","0","0","0","0","0","0",
         "0","0","0","0","0","0","0","0",
         "0","0","0","0","0","0","0","0",
         "0","0","1","0","0","0","0","0",
         "0","0","1","0","0","0","0","0",
         "0","0","1","0","0","0","0","0",
         "0","0","1","0","0","0","0","0",
         "0","0","1","0","0","0","0","0",
         ]

HOST = '127.0.0.1'    # The remote host
PORT = 50007 # The same port as used by the server
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.connect((HOST, PORT))
msg = sys.argv[1]
s.send(msg)
data = s.recv(1024)
print msg
s.close()
print 'Received', repr(data)
