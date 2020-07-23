# Klasse für Ereignise
class Event(object):

    def __init__(self):
        # Initialisiert die Liste der Handler auf null
        self.handlers = []

    def add(self, handler):
        # fügt einen Handler ein
        self.handlers.append(handler)
        return self

    def remove(self, handler):
        # entfernt einen Handler
        self.handlers.remove(handler)
        return self

    def fire(self, sender, earg=None):
        # ruft alle Handler auf, wenn das Ereignis ausgelöst wird
        for handler in self.handlers:
            handler(sender, earg)

    # Überschreibt die Standard Add Methode
    __iadd__ = add

    # Überschreibt die Standard Entferne Methode
    __isub__ = remove

    # Überschreibt die Standard Call Methode
    __call__ = fire
