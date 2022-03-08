from typing import Dict
from .base import BaseFormatter


class MboxFormatter(BaseFormatter):

    def __init__(self):
        super().__init__()


    def format(self, report: Dict) -> str:

        if self._get_short_type(report) != 'mbox':
            return ''

