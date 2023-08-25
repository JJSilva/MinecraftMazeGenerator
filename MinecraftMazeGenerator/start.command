#!/bin/bash
cd "$(dirname "$0")"
rm fifo
mkfifo fifo
tail -F fifo | java -Xms2G -Xmx2G -jar paper.jar &