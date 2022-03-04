import asyncclick as aclick
from flow.scan import ScanFlow

@aclick.group(name='system')
def system():
    pass


@system.command('sysinfo', short_help='Get system information')
async def sysinfo():
    pass


@system.command('sysconfig', short_help='Get system configuration')
async def sysconfig():
    pass
