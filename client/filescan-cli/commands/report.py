import asyncclick as aclick

@aclick.group(name='report')
def report():
    pass

@report.command('print', short_help='print me')
async def print():
    aclick.echo('Hello')

