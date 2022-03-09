from typing import Dict, List
from .features.base import BaseFormatter
from .features.overview import OverviewFormatter
from .features.details import DetailsFormatter
from .features.emulation import EmulationFormatter
from .features.iocs import IOCFormatter


class ReportFormatter:

    def __init__(self):
        self.formatters: List[BaseFormatter] = [
            OverviewFormatter(),
            DetailsFormatter(),
            EmulationFormatter(),
            IOCFormatter()
        ]


    def format(self, report: Dict) -> str:

        result = ''
        for formatter in self.formatters:
            result += formatter.format(report)

        return result



