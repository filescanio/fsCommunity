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


    async def get_scan_reports(self, scan_id: str, filters: Tuple, sorts: Tuple, graph: bool) -> Dict:

        spinner = Halo(text=f'Fetching reports ... ', placement='right')
        spinner.start()

        scan_report = await self.report.get_scan_reports(scan_id, filters, sorts, graph)

        if 'allFinished' in scan_report and not scan_report['allFinished']:
            spinner.warn('Not finished yet')
            return

        if 'reports' not in scan_report or not scan_report['reports']:
            spinner.fail('No data')
            return

        reports: Dict = scan_report['reports']
        spinner.succeed()


    async def get_report(self, id: str, hash: str, filters: Tuple, sorts: Tuple, graph: bool) -> Dict:

        spinner = Halo(text=f'Fetching a report ... ', placement='right')
        spinner.start()

        report = await self.report.get_report(id, hash, filters, sorts, graph)

        if not report:
            spinner.fail()
        else:
            spinner.succeed()


    async def get_reports(self, page: int, size: int) -> List:

        spinner = Halo(text=f'Fetching reports ... ', placement='right')
        spinner.start()

        reports = await self.report.get_reports(page, size)

        if not reports:
            spinner.fail()
        else:
            spinner.succeed()

        self.__view_reports(reports)


    async def search(self, params: Dict[str, str]) -> List:

        spinner = Halo(text=f'Searching reports ... ', placement='right')
        spinner.start()

        reports = await self.report.search_reports(params)

        if reports is None:
            spinner.fail()
        else:
            spinner.succeed()

        if not reports:
            return

        self.__view_reports(reports)


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


    def __view_reports(self, reports: List):
        """View reports"""

        for report in reports:
            self.__view_report(report)


    def __view_report(self, report: Dict):
        """View a report"""

        self.logger.debug(self.__format_report(report))


    def __format_report(self, report: Dict) -> str:

        return f'''
            id: {colorama.Fore.GREEN + report['id'] + colorama.Fore.WHITE}
            name: {colorama.Fore.GREEN + report['file']['name'] + colorama.Fore.WHITE}
            type: {report['file']['short_type']}
            hash: {report['file']['sha256']}
            verdict: {self.__format_verdict(report['verdict'])}
            tags: {' '.join([self.__format_tag(tag) for tag in report['tags']])}
            updated: {report['updated_date']}
        '''


    def __format_verdict(self, verdict: str) -> str:
        return get_verdict_color(verdict) + verdict + colorama.Fore.WHITE


    def __format_tag(self, tag: Dict) -> str:
        verdict = tag['tag']['verdict']['verdict']
        return get_verdict_color(verdict) + tag['tag']['name'] + colorama.Fore.WHITE
