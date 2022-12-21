<a href="esilv.fr" target="_blank">
<img src="https://upload.wikimedia.org/wikipedia/commons/d/d1/Logo_esilv_png_blanc.png" alt="Logo of the project" align="right" height="75rem">
</a>

# Image-processing-tool &middot; ![GitHub](https://img.shields.io/github/license/atinyzen/Image-processing-tool) ![GitHub last commit](https://img.shields.io/github/last-commit/atinyzen/Image-processing-tool?color=%2345f14d)
#### An image processing tool written in C# as a project for my 2nd year of engineering school.
<br>

This project was given to us, engineering students, as an end of semester project to gauge what we had learnt in our Object-Oriented Programming class. The image processing tool can be used to:
* Apply filters to pictures with convolution matrices
* Rotate, mirror, resize images
* Generate histograms for a picture
* Hide an image within another (steganography)
* Generate a QR Code (v1 & v2) from a sentence

```diff
- ‼️ All of these only work with Bitmap images (.bmp) ‼️ -
```

## Watch how it works

1. Open a Bitmap image to work with

![Alt Text](https://media0.giphy.com/media/1GrzYY0YlgunN8JdpB/giphy.gif?cid=5e214886ff74e6dd718468d8e77e9c44ca12061ebf26a776&rid=giphy.gif&ct=g)

2. Choose an action to apply to the picture ; here, there is a hidden image so I'll try to decode it

![Alt Text](https://media3.giphy.com/media/Q5KXq8eZKRYXJy26Vr/giphy.gif?cid=5e214886ccb27574826e8340f8791de283486c6d16a2ab4d&rid=giphy.gif&ct=g)

3. Take a look at the generated result

![Alt Text](https://media2.giphy.com/media/XmHBAyOuC3Se7E7jMV/giphy.gif?cid=5e214886b5b46740724bef07f30c9d788996a4617faecf7b&rid=giphy.gif&ct=g)

4. Save the picture (or not 😅)

![Alt Text](https://media1.giphy.com/media/fnP71mKgmX8oXwtDQU/giphy.gif?cid=5e2148863a51b16ce9630c7033144009a23463563cf79028&rid=giphy.gif&ct=g)

Basically, open an image and click on the button that fits what you want to do the most

## How to install this project

```shell
git clone https://github.com/atinyzen/Image-processing-tool.git
cd Image-processing-tool/
packagemanager install
```

## How to tweak this project for your own uses

As this project is under the [MIT](https://choosealicense.com/licenses/mit/) license, you can clone this project and rename this project to use for your own purposes !

## Found a bug ?

If you found an issue or would like to submit an improvement to this project, please submit an issue using the "Issues" tab just above. If you would like to submit a PR with a fix, reference the issue you created !

## Known issues

For the rotation functionality, there are some angles with which the rotated image kind of (really) looks off...
