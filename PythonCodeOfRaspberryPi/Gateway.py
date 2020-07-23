import glob
import threading
from tkinter import *
import time
import serial
import json

from EventManager import Event


class Gateway(threading.Thread):

    def __init__(self):
        threading.Thread.__init__(self)
        self.Connected = False
        self.SerialDevice = None
        self.FoundPorts = []
        self.PortComboBox = None
        self.IsSetup = False
        self.PortsScannedEvent = Event()
        self.IsStopped = False
        self.TestBtn = None
        self.RateCmbBx = None
        self.NextBtn = None
        self.ToSend = None
        sel.Server = None

    #Setup das Gateway mit dem Port des Arduinos, der Baurate der seriellen Kommunikation und dem Server
    def setup(self, port, band,server=None):

        try:
            self.SerialDevice = serial.Serial(port, band, timeout=.1)
            self.IsSetup = True
            self.NextBtn.configure(state=NORMAL)
            self.Server = server
        except:
            self.IsSetup = False
            self.NextBtn.configure(state=DISABLED)
            self.Server = server

    def Setup(self):
        port = self.PortComboBox.get()
        rate = self.RateCmbBx.get()
        if str(port) == '':
            self.IsSetup = False
            self.NextBtn.configure(state=DISABLED)
            return
        else:
            return self.setup(port, rate)

    def run(self):
        while not self.IsStopped:
            received = []
            if not (self.PortComboBox is None):
                self.getAllPorts()
                self.PortComboBox['values'] = self.FoundPorts
                if len(self.FoundPorts) > 0:
                    self.PortComboBox.set(self.FoundPorts[0])
                    self.TestBtn.configure(state=NORMAL)
                else:
                    self.PortComboBox.set('')
                    self.TestBtn.configure(state=DISABLED)
            if self.IsSetup:
                if self.SerialDevice.inWaiting():
                    data = ''
                    currChar =''
                    while not (currChar== '\n'):
                        data += currChar
                        currChar=self.SerialDevice.read().decode('utf8','ignore')
                    try:
                        
                        position = str(data.split(';')[0])
                        motion1 = str(data.split(';')[1])
                        actualEtage= str(data.split(';')[2])
                        print("Position : "+position+"\nMotion1 : "+motion1+"\nActualEtage :"+actualEtage)
                        if !(self.Server is None):
                            elevator = server.User.ElevatorN
                            elevator.Distance = position
                            elevator.ActualEtage = actualEtage
                            elevator.MotionDetector1 = !(motion1 == 0)
                            updateElevatorInfo(elevator)
                        #print(data)
                    except Exception as er:
                        print(str(er))
                        pass

                        
                    
                    time.sleep(1)
                    

    def getAllPorts(self):
        if sys.platform.startswith('win'):
            ports = ['COM%s' % (i + 1) for i in range(256)]
        elif sys.platform.startswith('linux') or sys.platform.startswith('cygwin'):
            # this excludes your current terminal "/dev/tty"
            ports = glob.glob('/dev/tty[A-Za-z]*')
        elif sys.platform.startswith('darwin'):
            ports = glob.glob('/dev/tty.*')
        else:
            raise EnvironmentError('Unsupported platform')

        result = []
        for port in ports:
            try:
                s = serial.Serial(port)
                s.close()
                result.append(port)
            except (OSError, serial.SerialException):
                pass
        self.FoundPorts = result
