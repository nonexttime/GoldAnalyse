import re
import urllib2
import thread

#Global

page3 = urllib2.urlopen("http://gold.hexun.com/hjxh/").read()
p3 = re.findall("<td([^<]*)</td>",page3)

#Price Func
def getPrice_Buy():
    return p3[2][11:]

def getPrice_Buy():
    return p3[3][11:]

def getPrice_Change():
    return p3[4][1:]

def getPrice_ChangePer():
    return p3[5][1:][:-1]

def getPrice_Time():
    return p3[6][1:]

#International_Price Func
def getIntPrice_Buy():
    return p3[7][11:]

def getIntPrice_Sell():
    return p3[8][11:]

def getIntPrice_Change():
    return p3[9][1:]

def getIntPrice_ChangePer():
    return p3[10][1:][:-1]

def getIntPrice_Time():
    return p3[11][1:]
