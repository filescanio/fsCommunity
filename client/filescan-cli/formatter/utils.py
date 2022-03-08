from typing import Dict
from common.colors import colorize, get_verdict_color
import math


def format_dict(dict: Dict) -> str:

    result = ''
    for key in dict:
        name = ' '.join(map(lambda substr: substr.capitalize(), key.split('_')))
        result += f'''
                {name}: {dict[key]}'''

    return result + '\n'


def format_verdict(verdict: str) -> str:
    return colorize(verdict, get_verdict_color(verdict))


def format_tag(tag: Dict) -> str:
    verdict = tag['tag']['verdict']['verdict']
    return colorize(tag['tag']['name'], get_verdict_color(verdict))


def format_size(size: int) -> str:
    idx = math.floor(math.log(size) / math.log(1024))
    return "{:.2f}".format(size / math.pow(1024, idx)) + ['B', 'kB', 'MB', 'GB', 'TB'][idx]
