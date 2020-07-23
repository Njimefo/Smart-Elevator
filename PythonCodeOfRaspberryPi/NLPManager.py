import requests
from Extensions import *

class NLP:
    def __init__(self, key='DQKG4RLPPSBUCJEOR72RWDRIBU36XC3B'):
        self.apiKey = key

    def ProcessWrittenText(self, text):
        url = "https://api.wit.ai/message?v=20140401&q=" + text
        headers = {"Authorization": "Bearer " + self.apiKey}
        rq = requests.get(url, headers=headers)
        jsonResponse = rq.json()

        response = objFromDictionary(jsonResponse)

        return response


