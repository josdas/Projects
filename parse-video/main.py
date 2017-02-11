import pylab
import imageio
import numpy as np

images = []
alp = ['A', 'B', 'D', 'G', 'I', 'M', 'O', 'R', 'T', 'U', 'V', 'W', 'X', 'Y']
def getAbs(image):#348
    minx = miny = 10**5
    maxx = maxy = -10**5
    wb = [0, 255]
    if image[0][0][0] > 128:
       wb[0], wb[1] = wb[1], wb[0]
    for c in range(len(image)):
        for d in range(len(image[c])):
            if image[c][d][0] == wb[1]:
                minx = min(minx, c)
                miny = min(miny, d)
                maxx = max(maxx, c)
                maxy = max(maxy, d)
    newImage = [[0] * (maxy - miny + 1) for i in range(maxx - minx + 1)]
    for c in range(minx, maxx + 1):
        for d in range(miny, maxy + 1):
            newImage[c - minx][d - miny] = image[c][d]
    return newImage
def getGrey(image):
    gr = [[0] * len(image[0]) for i in range(len(image))]
    wb = [0, 255]
    for c in range(len(image)):
        for d in range(len(image[c])):
            for i in range(3):
                gr[c][d] += image[c][d][i]
            if gr[c][d] > 190 * 3:
                gr[c][d] = wb[0]
            else:
                gr[c][d] = wb[1]
    return gr

def getImage(image):
    gr = getGrey(image)
    for c in range(len(image)):
        for d in range(len(image[c])):
            for i in range(3):
                image[c][d][i] = gr[c][d]
    return image

def resize(image, h, w):
    newImage = [[0] * w for i in range(h)]
    H = len(image)
    W = len(image[0])
    for i in range(h):
        for j in range(w):
            newImage[i][j] = image[i * H // h][j * W // w]
    return newImage

def getEqual(imageA, imageB):
    s = 0
    for i in range(len(imageA)):
        for j in range(len(imageA[0])):
            if imageA[i][j][0] == imageB[i][j][0]:
                s += 1
    return s
GG = []
def getLetter(image, NUM):
    cof = 0
    ch = 0
    T = []
    for l in range(len(alp)):
        d = getEqual(image, images[l])
        if cof < d:
            cof = d
            ch = alp[l]
        T.append(-d)
        #print(alp[l], d)
    T.sort()
    if -T[0] + T[1] < 50:
        print(T)
        print(NUM)
        GG.append(NUM)
        return 0
    return ch

def main():
    for a in alp:
        vid = imageio.get_reader(a + '.jpg', 'JPEG')
        image = vid.get_data(0)

        image = getImage(image)
        image = getAbs(image)
        image = resize(image, 40, 40)

        images.append(image)


        #fig = pylab.figure()
        #fig.suptitle('image #{' + a + '}', fontsize = 10)
        #pylab.imshow(image)

    vid = imageio.get_reader('TaskB.mp4', 'ffmpeg')

    dictOfLetters = {}
    for num in range(242, vid.get_length(), 4):
        image = vid.get_data(num)

        image = resize(image, 200, 200)

        image = getImage(image)
        image = getAbs(image)
        image = resize(image, 40, 40)

        letter = getLetter(image, num)
        if letter not in dictOfLetters:
            dictOfLetters[letter] = 0
        dictOfLetters[letter] += 1

        if num % 500 == 2:
            print(num, dictOfLetters)

        print(letter)

        #fig = pylab.figure()
        #fig.suptitle('image ' + letter + ' ' + str(num), fontsize = 20)
        #pylab.imshow(image)
    print(dictOfLetters)
    #pylab.show()

"""E = {'W': 495, 'B': 110, 'G': 62, 'V': 250, 'M': 6, 'R': 51, 'Y': 309, 'A': 247, 'T': 131, 'X': 202, 'I': 584, 'O': 57, 'U': 288, 'D': 238}
W = []
for a, b in E.items():
    W.append([a, b])
W.sort(key = lambda e: e[0])
s = ''
for i in W:
    s += str(i[0]) + str(i[1]);
#print(s)
main()#[11258, 10954, 10646, 10558, 9778,  9586, 8478, 8442, 7682, 7106, 6834, 5738, 5602, 5310, 3958, 3802 ,3698 ,3166, 3050, 3038 ,2986 ,2678 ,2486 ,2106 ,1542 ,1386 ,1102]"""
























