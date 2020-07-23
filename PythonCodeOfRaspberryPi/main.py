from tkinter import messagebox

from GUI import *
from Gateway import Gateway
from Server import Server
from EventManager import Event
global root, main, server, gateway


# server = Server('http://home.htw-berlin.de/~s0554918/SmartElevator')
# result = server.login('Brandon', 'Brandon360')
# print(result)
# elevator = server.User.ElevatorN
# elevator.Distance = 80
# server.updateElevatorInfo(elevator)

if __name__ == "__main__":
    root = Tk()
    main = MainView(root)
    main.pack(side="top", fill="both", expand=True)
    root.wm_geometry("500x200")
    root.mainloop()
