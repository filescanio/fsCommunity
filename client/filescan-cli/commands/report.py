import asyncclick as aclick
from flow.report import ReportFlow

@aclick.group(name='report')
def report():
    pass

@report.command('report', short_help='Get reports or report related to scan or id')
@aclick.option('--flow', type=str, is_flag=False, default='', help='Flow id')
@aclick.option('--id', type=str, is_flag=False, default='', help='''
Report id: Must be given when flow option is absent, or will be ignored
''')
@aclick.option('--hash', type=str, is_flag=False, default='', help='''
File hash the report contains. Must be given when id is specified
''')
@aclick.option('-f', '--filters', type=str, is_flag=False, multiple=True, default=[], help='Filters that apply to the report')
@aclick.option('-s', '--sorts', type=str, is_flag=False, multiple=True, default=[], help='Sorting that apply to the report')
@aclick.option('-g', '--graph', type=bool, is_flag=True, default=False, help='Whether get emulation graph or not')
async def get_report(
    flow: str,
    id: str,
    hash: str,
    filters: list[str],
    sorts: list[str],
    graph: bool
):
    report_flow = ReportFlow()
    if flow:
        await report_flow.get_scan_reports(flow, filters, sorts, graph)
    elif id and hash:
        await report_flow.get_report(id, hash, filters, sorts, graph)
