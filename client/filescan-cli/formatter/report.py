from typing import Dict, Any, List
from common.colors import get_verdict_color, colorize
from .features.overview import OverviewFormatter


class ReportFormatter:

    def __init__(self):
        self.formatters = [
            OverviewFormatter()
        ]


    def format(self, report: Dict) -> str:

        result = ''
        for formatter in self.formatters:
            result += formatter.format(report)

        return result


    def __get_resources(self, report: Dict) -> Dict:
        return report['resources'] if 'resources' in report else {}


    def __get_resource(self, report: Dict, type: str) -> Dict:
        resources = self.__get_resources(report)
        for key in resources:
            resource = resources[key]
            if 'resourceReference' not in resource:
                continue
            if 'name' not in resource['resourceReference']:
                continue
            if resource['resourceReference']['name'] == type:
                return resource
