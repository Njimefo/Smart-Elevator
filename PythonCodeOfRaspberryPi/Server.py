import json

import requests

from Extensions import objFromDictionary


class Server(object):
    
    def __init__(self, baseUri):
        self.BaseUri = baseUri
        self.User = None
        self.LoggedIn = False
        return super(Server, self).__init__()
    
    #registriert einen User
    def register(self, username, password):
        path = self.BaseUri + "/register.php"
        rq = requests.post(path, json={'Username': username, 'Password': password}, )
        result = rq.json()
        print(result)

    #loggt einen User ein
    def login(self, username, password):
        path = self.BaseUri + "/login.php"
        rq = requests.post(path, json={'Username': username, 'Password': password}, )
        result = rq.json()
        print(result)
        obj = objFromDictionary(result)
        if ((
                    obj.Executed or not obj.Executed) and obj.Result is None) or not obj.Result.LoggedIn or obj.Result.User is None:
            self.LoggedIn = False
            return False
        else:
            if type(obj.Result.User.ElevatorN) is str:
                obj.Result.User.ElevatorN = objFromDictionary(json.loads(obj.Result.User.ElevatorN))
            self.User = obj.Result.User
            self.LoggedIn = True
            return True

    # Befehl geben, dass sich der Fahrstuhl zur Etage <<goto>> gehen muss
    def updateGoto(self, goto):
        if not self.LoggedIn:
            return
        path = self.BaseUri + "/users.php"
        rq = requests.post(path,
                           json={'Username': self.User.Username, 'Token': self.User.CurrentToken.Value, 'Action': 1,
                                 'Goto': goto})
        result = rq.json()
        print(result)
        self.User.ElevatorN=goto

    #aktualisiert die Informationen des Fahrstuhls
    def updateElevatorInfo(self, elevator):
        if not self.LoggedIn:
            return
        path = self.BaseUri + "/users.php"
        rq = requests.post(path,
                           json={'Username': self.User.Username, 'Token': self.User.CurrentToken.Value, 'Action': 0,
                                 'Elevator': elevator.toUsableJsonObject()})
        result = rq.json()
        print(result)
        self.User.ElevatorN=elevator
