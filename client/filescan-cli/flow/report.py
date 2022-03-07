import aiofiles
import colorama
from typing import List, Dict, Tuple
from core.logger import Logger
from halo import Halo
from service.report import Report
from common.colors import get_verdict_color

class ReportFlow:

    def __init__(self):
        self.report = Report()
        self.logger = Logger()


    async def get_report(self, id: str, hash: str, filters: Tuple, sorts: Tuple, graph: bool) -> Dict:

        spinner = Halo(text=f'Fetching a report ... ', placement='right')
        spinner.start()

        report = await self.report.get_report(id, hash, filters, sorts, graph)

        if not report:
            spinner.fail()
        else:
            spinner.succeed()


    async def get_formatted_report(self, report_id: str, format: str, output):

        spinner = Halo(text=f'Getting {format}-formatted report ... ', placement='right')
        spinner.start()

        report = await self.report.download_report(report_id, format)

        if report:
            spinner.succeed()
            async with aiofiles.open(output, 'w+' if isinstance(report, str) else 'wb') as writer:
                await writer.write(report)
        else:
            spinner.fail()
