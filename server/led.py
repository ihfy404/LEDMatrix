#!/usr/local/bin/python
# -*- coding: utf-8 -*-
 
import RPi.GPIO as GPIO
import time
import sys
import string
import socket
import threading
import datetime
 
HOST = '127.0.0.1'
PORT = 50007
INTERVAL = 1 # 測定間隔
 
status = { "result" : "" } # 結果保存用
 
# 各GPIOの番号
SI = 25
RCK = 24
SCK = 23

#param = [1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1]
param = [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]
GPIO.setmode(GPIO.BCM)
 
# 初期化
GPIO.setup(SCK, GPIO.OUT)
GPIO.setup(RCK, GPIO.OUT)
GPIO.setup(SI, GPIO.OUT)
 
def reset():
  GPIO.output(SCK, GPIO.LOW)
  GPIO.output(RCK, GPIO.LOW)
  GPIO.output(SI, GPIO.LOW)
 
def shift(PIN_NUM):
  GPIO.output(PIN_NUM, GPIO.HIGH)
  GPIO.output(PIN_NUM, GPIO.LOW)
 
def send_bits(data):
  for i in range(64):
    if ((1 << i ) & data) == 0:
      GPIO.output(SI, GPIO.LOW)
    else:
      GPIO.output(SI, GPIO.HIGH)
    shift(SCK)
 
  shift(RCK)
 
def lighting():
  state = param
  idx = 0
  GPIO.output(RCK, GPIO.LOW)
  GPIO.output(SCK, GPIO.LOW)

  for i in range(64):
    if((state[idx] == "1") or (state[idx] == 1) ):
      GPIO.output(SI, GPIO.HIGH)
    else:
      GPIO.output(SI, GPIO.LOW)
    idx = idx + 1
    GPIO.output(SCK, GPIO.HIGH)
    GPIO.output(SCK, GPIO.LOW)
    #shift(SCK)
  #shift(RCK)
  GPIO.output(RCK, GPIO.HIGH)
  GPIO.output(RCK, GPIO.LOW)

def all_off():
  for i in range(64):
    GPIO.output(SI, GPIO.LOW)
    shift(SCK)
    shift(RCK)

# 測定実行用スレッドのクラス
class MyThread(threading.Thread):
 
    def __init__(self):
        super(MyThread, self).__init__()
        self.setDaemon(True)
 
    def run(self):
        while True:
          start = time.time()
          lighting()
          elapsed_time = time.time() - start
          #print ("lighting time{0}".format(elapsed_time)+ "[sec]")

 
# サーバを作成して動かす関数
def socket_work():
     
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.bind((HOST, PORT))
    s.listen(1)
 
    while True:
        conn, addr = s.accept()
        print 'Connected by', addr
        data = conn.recv(1024)
        global param
        param = data.split(',')
        print data
        conn.send(status["result"])
        conn.close()

try:
  #print("start")
  #param = sys.argv[1].split(',')
  # スレッドの作成と開始
  mythread = MyThread()
  mythread.start()

  socket_work() 
except KeyboardInterrupt:
  reset()
  GPIO.cleanup()
