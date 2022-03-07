import asyncclick as aclick
from flow.system import SystemFlow

@aclick.group(name='system')
def system():
    pass


@system.command('sysinfo', short_help='Get system information')
async def sysinfo():
    system_flow = SystemFlow()
    await system_flow.get_info()


@system.command('sysconfig', short_help='Get system configuration')
async def sysconfig():
    system_flow = SystemFlow()
    await system_flow.get_config()
