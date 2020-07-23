import speech_recognition as sr

from EventManager import Event


class SpeechRecognizer:
    def __init__(self, key="DQKG4RLPPSBUCJEOR72RWDRIBU36XC3B"):
        self.recognizer = sr.Recognizer()
        self.Key = key
        self.RecognizationStarted = Event()
        self.RecognizationEnded = Event()

    def StartSpeech(self):
        resultMessage = None
        with sr.Microphone() as source:
            audio = self.recognizer.listen(source)
            self.RecognizationStarted.fire(self, None)
        try:
            result = self.recognizer.recognize_wit(audio, key=self.Key)
            self.RecognizationEnded.fire(self, None)
        except sr.UnknownValueError:
            result = None
            resultMessage = "could not understand the audio"
        except sr.RequestError as e:
            result = None
            resultMessage = "Could not request results from Wit.ai service; {0}".format(e)
        return result, resultMessage
