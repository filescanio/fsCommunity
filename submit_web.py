#!/usr/bin/env python

####
# FileScanIO Webservice Sample Feeder Script
#
# Date: 08-17-2021
# Changes:
#  - added multi-threading and 'FSIO_MAX_WORKERS' setting
#
# Date: 08-10-2021
# Changes:
#  - initial version
####

import re
import pprint
import sys
import requests
import json
from time import time
import os
import traceback
import hashlib
import fnmatch
from concurrent.futures import ThreadPoolExecutor, as_completed

#
# Configuration
#
FSIO_WEB_APIKEY = ""
FSIO_WEB_ENDPOINT = "https://production.filescan.io/"
FSIO_MAX_WORKERS = 10

#
# Advanced Settings
#
FSIO_WEB_USERAGENT = "FileScanIO Python"
FSIO_WEB_ENDPOINT_SUBMIT_FILE = FSIO_WEB_ENDPOINT + "/api/scan/file"
FSIO_WEB_HEADERS_SUBMIT = {
    'accept': 'application/json',
    'User-Agent': FSIO_WEB_USERAGENT,
    'X-Api-Key': FSIO_WEB_APIKEY,
    'description': '',
    'password': '',
    'tags': '',
    'propagate_tags': 'true',
    'is_private': 'false',
}

pp = pprint.PrettyPrinter(indent=4)

def main():
    print("FileScanIO Transform Server Upload Script")
    if len(sys.argv) == 2:
        submit(sys.argv[1], None)
    elif len(sys.argv) == 3:
        submit(sys.argv[1], sys.argv[2])
    else:
        print("Usage: submit_web <file or directory> [pattern]")

def submit(path, pattern):
    start = time()
    processes = []
    with ThreadPoolExecutor(max_workers=FSIO_MAX_WORKERS) as executor:
        _submit(path, pattern, processes, executor)
    for task in as_completed(processes):
        print(task.result())
    print(f'Time taken: {time() - start}')

def _submit(path, pattern, processes, executor):
    start = time()
    print("[FSIO_WEB] Processing",path,"...")
    if os.path.isdir(path):
        f_names = os.listdir(path)
        for f_name in f_names:
            f_name = os.path.join(path, f_name)
            if not os.path.isdir(f_name):
                if pattern is not None and not fnmatch.fnmatch(f_name, pattern):
                    continue
                processes.append(executor.submit(post, f_name))
            else:
                _submit(f_name, pattern, processes, executor)
    else:
        post(path)

def post(f_name):
    try:
        print("[FSIO_WEB] Uploading ",f_name," ...")
        files = {'file':(os.path.basename(f_name), open(f_name,'rb'), 'application/octet-stream')}
        response = requests.post(FSIO_WEB_ENDPOINT_SUBMIT_FILE, headers=FSIO_WEB_HEADERS_SUBMIT, data={}, files=files, verify=False)
        if response.status_code == 200 or response.status_code == 201:
            print("[FSIO_WEB] Submitted file to your FilescanIO webservice instance successfully: {}\n[Your SHA256: {}]".format(
                os.path.basename(f_name),
                hashlib.sha256(open(f_name, "rb").read()).hexdigest()))
        else:
            print("[FSIO_WEB] Error. Response code: ",response.status_code)
        return pp.pprint(response.content)
        
    except Exception:
        print("%s %s" % (sys.stderr, traceback.format_exc()))

if __name__=="__main__":
    main()
