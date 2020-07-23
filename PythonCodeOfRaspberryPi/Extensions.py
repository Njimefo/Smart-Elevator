import json


# Personalisiertes Element
# Es dient dazu, ein Klassenobjekt aus einem Dictionnary zu erstellen
class objFromDictionary(object):

    # Konstruktor zur Initialisierung
    #
    def __init__(self, d):
        for a, b in d.items():
            if isinstance(b, (list, tuple)):
                setattr(self, a, [objFromDictionary(x) if isinstance(x, dict) else x for x in b])
            else:
                setattr(self, a, objFromDictionary(b) if isinstance(b, dict) else b)

    # Konvertiert das Objekt in Json Format
    def toJson(self):
        return json.dumps(self, default=lambda o: o.__dict__,
                          sort_keys=True, indent=4)
    #Da Python benutzerdefinierte Objekte selber nicht deserializieren oder serializieren kann
    #muss man erstmal das Objekt selbst über die toJson Methode das Objekt in Json String umwandeln
    #Danach kann man den Result String in ein Objekt konvertieren, mit dem Python umgehen kann 
    def toUsableJsonObject(self):
        return json.loads(self.toJson())
