from typing import Dict
from common.colors import get_verdict_color, colorize


class ReportsFormatter:

    def __init__(self):
        pass


    def format(self, reports: list) -> str:

        result = ''
        for report in reports:
            result += self.__format_report(report)

        return result


    def __format_report(self, report: Dict) -> str:

        return f'''
            id: {report['id']}
            name: {colorize(report['file']['name'])}
            type: {report['file']['short_type']}
            hash: {report['file']['sha256']}
            verdict: {self.__format_verdict(report['verdict'])}
            tags: {' '.join([self.__format_tag(tag) for tag in report['tags']])}
            updated: {report['updated_date']}
        '''


    def __format_verdict(self, verdict: str) -> str:
        return colorize(verdict, get_verdict_color(verdict))


    def __format_tag(self, tag: Dict) -> str:
        verdict = tag['tag']['verdict']['verdict']
        return colorize(tag['tag']['name'], get_verdict_color(verdict))
