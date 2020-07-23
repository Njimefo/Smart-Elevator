# Dieses Modeul soll alle GUI bezogene Elemente haben

from tkinter import messagebox

from GUI import *
from Gateway import Gateway
from Server import Server
from EventManager import Event
import tkinter as tk
from tkinter import *
from tkinter.ttk import *
from NLPManager import NLP
from SpeechRecognition import SpeechRecognizer
global root, main, server, gateway


# Klasse für die Seitenvorlage
class Page(Frame):
    def __init__(self, *args, **kwargs):
        Frame.__init__(self, *args, **kwargs)

    # Standardfunktion zum Anzeigen der Seite
    def show(self):
        self.lift()


# Die Verarbeitungsseite:
# Auf dieser soll es möglich sein Befehle an den Fahrstuhl sprachlich oder schriftlich zu senden
class Processingpage(Page):

    # Konstruktor zur Initialisierung der Steuerelemente der Seite
    def __init__(self, *args, **kwargs):
        Page.__init__(self, *args, **kwargs)

        _label1 = tk.Label(self, text="Geben Sie etwas ein")
        _label1.grid(column=0, row=0)

        self.InstructionEntry = tk.Entry(self, width=60)
        self.InstructionEntry.grid(column=0, row=1)

        self.SendBtn = tk.Button(self, text="Senden", width=10, bg="gray")
        self.SendBtn.grid(column=1, row=1, sticky=W, padx=2)

        _label2 = tk.Label(self, text="Ihre Eingabe")
        _label2.grid(column=0, row=2)

        self.ResultInstructionEntry = tk.Label(self, width=60, bg='gray')
        self.ResultInstructionEntry.grid(column=0, row=3)

        self.SpeechBtn = tk.Button(self, text="Start Speech", width=10, bg='gray')
        self.SpeechBtn.grid(column=1, row=3, sticky=N, padx=2)

        self.OnSpeechCan = Canvas(self, state=DISABLED)
        self.OnSpeechCan.create_oval(5, 5, 30, 30, outline="#f11",
                                     fill="green", width=1)
        self.OnSpeechCan.grid(column=0, row=5)

        self.SpeechEndCan = Canvas(self, state=NORMAL)
        self.SpeechEndCan.create_oval(5, 5, 30, 30, outline="#f11",
                                      fill="red", width=1)
        self.SpeechEndCan.grid(column=0, row=5)


# Startseite, über die man sich an den Server verbinden kann
class Startpage(Page):
    def __init__(self, *args, **kwargs):
        Page.__init__(self, *args, **kwargs)

        nameLabel = tk.Label(self, text="Url des Servers")
        nameLabel.grid(column=0, row=0)

        self.UrlEntry = tk.Entry(self, width=60)
        self.UrlEntry.grid(column=0, row=1)

        usernameLbl = tk.Label(self, text="Benutzername")
        usernameLbl.grid(column=0, row=2)

        self.UsernameEntry = tk.Entry(self, width=60)
        self.UsernameEntry.grid(column=0, row=3)

        passwordLbl = tk.Label(self, text="Passwort")
        passwordLbl.grid(column=0, row=4)

        self.PasswordEntry = Entry(self, width=60)
        self.PasswordEntry.grid(column=0, row=5)

        self.LogBtn = tk.Button(self, text="Einloggen", width=40, bg='gray')
        self.LogBtn.grid(column=0, row=6)

        self.NextBtn = tk.Button(self, pady=0, text="Weiter", width=40, bg='green', state=tk.DISABLED)
        self.NextBtn.grid(column=0, row=7)


# Seite zum Verbinden des Raspberry Pi mit dem Arduino über Serielle Kommunikation
class SerialPage(Page):
    def __init__(self, *args, **kwargs):
        Page.__init__(self, *args, **kwargs)

        urlLabel = tk.Label(self, text="Port")
        urlLabel.grid(column=0, row=0)

        self.PortCmbBx = Combobox(self, width=60, state="readonly")
        self.PortCmbBx.grid(column=0, row=1)

        usernameLbl = tk.Label(self, text="Rate")
        usernameLbl.grid(column=0, row=2)

        self.RateCmbBx = Combobox(self, width=60, state="readonly",
                                  values=['9600', '19200', '38400', '57600', '115200'  '18432'])
        self.RateCmbBx.set('9600')
        self.RateCmbBx.grid(column=0, row=3)

        self.TestBtn = tk.Button(self, text="Test", width=40, bg='gray')
        self.TestBtn.grid(column=0, row=4)

        self.NextBtn = tk.Button(self, pady=0, text="Weiter", width=40, bg='green', state=tk.DISABLED)
        self.NextBtn.grid(column=0, row=5)


# Hauptseite, wo alle Seiten übereinander gestapelt werden und nach Bedarf einzel angezeigt werden
class MainView(Frame):

    # Konstruktor: Fügt die Seiten ein, und positioniert sie
    # Die Startseite als Erstes angezeigt
    def __init__(self, *args, **kwargs):
        Frame.__init__(self, *args, **kwargs)

        self.server = None
        self.gateway = None

        self.NewCommandGiven = Event()
        self.NewReservationGiven = Event()
        self.Recognizer = SpeechRecognizer()

        # erstellt die verschiedenen Seiten
        self.startPage = Startpage(self)
        self.serialPage = SerialPage(self)
        self.processingPage = Processingpage(self)
        self.processingPage.SpeechBtn.configure(command=self.startRecognition)

        # Container für die verschiedenen Seiten : Initialisierung
        container = Frame(self)
        container.pack(side="top", fill="both", expand=True)

        # Platzierung der Seiten
        self.startPage.place(in_=container, x=0, y=0, relwidth=1, relheight=1)
        self.serialPage.place(in_=container, x=0, y=0, relwidth=1, relheight=1)
        self.processingPage.place(in_=container, x=0, y=0, relwidth=1, relheight=1)

        # Setzen der Klick-Event Ereignisse
        self.startPage.LogBtn.configure(command=self.connect)
        self.startPage.NextBtn.configure(command=self.showSerial)
        self.serialPage.NextBtn.configure(command=self.showProcessing)
        self.processingPage.SendBtn.configure(command= self.PrcessSpokenText)


        # Startseite anzeigen
        self.startPage.show()
        #self.processingPage.show()

    # Seite für die Serielle Kommunikation anzeigen
    def showSerial(self):
        self.serialPage.show()

    def startRecognition(self):
        result, resultMessage = self.Recognizer.StartSpeech()
        if result != None:
            self.processingPage.ResultInstructionEntry.config(text=result)
            nlp = NLP()
            result = nlp.ProcessWrittenText(result)
            self.ProcessText(result)


    # Verarbeitungsseite anzeigen
    def showProcessing(self):
        self.processingPage.show()

    def speechStarted(self, sender, args):
        self.processingPage.OnSpeechCan.configure(state=NORMAL)
        self.processingPage.SpeechEndCan.configure(state=DISABLED)
        self.processingPage.SpeechBtn.configure(state=DISABLED, bg='green')

    def PrcessSpokenText(self):
        nlp = NLP()
        instruction= self.processingPage.InstructionEntry.get()
        self.processingPage.ResultInstructionEntry.config(text=instruction)
        result= nlp.ProcessWrittenText(instruction)
        self.ProcessText(result)


    def ProcessText(self,elt):
        try :
            if elt.outcome.entities.intent.value == 'command':
                self.updateElevatorCommand(elt.outcome.entities.to.value)
            elif elt.outcome.entities.intent.value == 'reservation':
                self.updateElevatorReservation(
                    {'date': elt.outcome.entities.datetime, 'to': elt.outcome.entities.to.value})
        except:
            pass

    def speechEnded(self, sender, args):
        self.processingPage.OnSpeechCan.configure(state=DISABLED)
        self.processingPage.SpeechEndCan.configure(state=NORMAL)
        self.processingPage.SpeechBtn.configure(state=NORMAL, bg="gray")


    def connect(self):
        baseUri = str(self.startPage.UrlEntry.get())
        username = str(self.startPage.UsernameEntry.get())
        password = str(self.startPage.PasswordEntry.get())
        if not bool(baseUri and baseUri.strip()) or not bool(username and username.strip()) or not bool(
                password and password.strip()):
            messagebox.showerror("Einloggen",
                                 "Ein oder mehrere Fehler sind aufgetreten.\nStellen Sie sicher, dass Sie alle Felder richtig ausgefüllt  und eine Internetverbindung haben")
            return

        self.server = Server(baseUri)
        self.server.login(username, password)
        if self.server.LoggedIn:
            self.startPage.NextBtn.configure(state=NORMAL)
            self.gateway = Gateway()
            self.gateway.TestBtn = self.serialPage.TestBtn
            self.gateway.PortComboBox = self.serialPage.PortCmbBx
            self.gateway.RateCmbBx = self.serialPage.RateCmbBx
            self.gateway.NextBtn = self.serialPage.NextBtn
            self.gateway.TestBtn.configure(command=self.gateway.Setup)
            self.gateway.start()
        else:
            self.startPage.NextBtn.configure(state=DISABLED)
            messagebox.showerror("Einloggen",
                                 "Einloggen Fehler.\nStellen Sie sicher, dass Sie Ihren Benutzernamen und Passwort eingetragen und eine Internetverbindung haben")


    def updateElevatorCommand(self, args):
        o = 1
        self.server.updateGoto(int(args))


    def updateElevatorReservation(self, args):
        o = 1

