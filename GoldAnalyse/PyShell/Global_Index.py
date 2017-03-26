import sys
sys.path.append(r"D:\tools\IronPython 2.7\Lib")
import re
import urllib2
import thread

#Global
page = urllib2.urlopen("http://app.finance.ifeng.com/hq/global_ind/index.php").read()
p1 = re.findall("<td>([^<]*)</td>",page)
page2 = urllib2.urlopen("http://app.finance.ifeng.com/hq/global_ind/index.php?area=asia").read()
p2 = re.findall("<td>([^<]*)</td>",page2)
page3 = urllib2.urlopen("http://gold.hexun.com/hjxh/").read()
p3 = re.findall("<td([^<]*)</td>",page3)

#DJI Func
def getDJI():
    return p1[21]

def getDJI_Change():
    return p1[22]

def getDJI_ChangePer():
    return p1[23][:-1]

def getDJI_Time():
    return p1[24]

#CCMP Func
def getCCMP():
    return p1[81]

def getCCMP_Change():
    return p1[82]

def getCCMP_ChangePer():
    return p1[83][:-1]

def getCCMP_Time():
    return p1[84]

#SCIN Func
def getSCIN():
    return p2[0]

def getSCIN_Change():
    return p2[1]

def getSCIN_ChangePer():
    return p2[2][:-1]

def getSCIN_Time():
    return p2[3]

def Test():
    print "DJI\t",getDJI(),getDJI_Change(),getDJI_ChangePer()+"\t",getDJI_Time()
    print "CCMP\t",getCCMP(),getCCMP_Change(),getCCMP_ChangePer()+"\t",getCCMP_Time()
    print "SCIN\t",getSCIN(),getSCIN_Change(),getSCIN_ChangePer()+"\t",getSCIN_Time()

#Price Func
def getPrice_Buy():
    return p3[2][11:]

def getPrice_Sell():
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
