from typing import Dict, List
from .features.base import BaseFormatter
from .features.overview import OverviewFormatter
from .features.details import DetailsFormatter
from .features.emulation import EmulationFormatter


class ReportFormatter:

    def __init__(self):
        self.formatters: List[BaseFormatter] = [
            OverviewFormatter(),
            DetailsFormatter(),
            EmulationFormatter(),
        ]


    def format(self, report: Dict) -> str:

        result = ''
        for formatter in self.formatters:
            result += formatter.format(report)

        return result



