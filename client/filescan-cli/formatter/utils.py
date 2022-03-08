from typing import Dict
from common.colors import colorize, get_verdict_color


def format_dict(dict: Dict) -> str:

    result = ''
    for key in dict:
        name = ' '.join(
            map(lambda substr: substr.capitalize(), key.split('_')))
        result += f'{name}: {dict[key]}'

    return result + '\n'


def format_verdict(verdict: str) -> str:
    return colorize(verdict, get_verdict_color(verdict))


def format_tag(tag: Dict) -> str:
    verdict = tag['tag']['verdict']['verdict']
    return colorize(tag['tag']['name'], get_verdict_color(verdict))
