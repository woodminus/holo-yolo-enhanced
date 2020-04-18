from keras import backend as K
import tensorflow as tf
config = tf.ConfigProto()
config.gpu_options.allow_growth=True
sess = tf.Session(config=config)
K.set_session(sess)

try:
    import simplejson as json
except ImportError:
    import json

from flask import Flask, jsonify, request
from yolo import YOLO
from PIL import Image
import base64, json
import io
import time
import base64

app = Flask(__name__)

yolo = None

def __initialize_model():
    global yolo
    yolo = YOLO()

__initialize_model()

@app.route("/image", methods=['POST'])
def image():
    start = time.perf_counter()
    payload = request.get_json()
    
    # data = bytes.fromhex(payload['data'])
    data = base64.b64decode(payload['data'])

    width = payload['width']
    height = payload['height']

    # image = Image.frombytes("RGBA", (width, height), bytes(data))
    image = Image.open(io.BytesIO(data))
    out_boxes, out_scores, out_classes = yolo.detect_boxes(image)

    out_boxes = out_boxes.astype('float64', copy=False)
    out_scores = out_scor