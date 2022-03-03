import asyncclick as aclick
from flow.report import ReportFlow
from core.logger import Logger

@aclick.group(name='reports')
def reports():
    pass

@reports.command('reports', short_help='Get reports')
@aclick.option('--page', type=int, is_flag=False, default=1, help='Page number')
@aclick.option('--count', type=int, is_flag=False, default=10, help='Page size')
async def get_reports(page, count):
    """Get reports"""

    report_flow = ReportFlow()
    await report_flow.get_reports(page, count)
