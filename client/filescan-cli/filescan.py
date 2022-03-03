import asyncio
import asyncclick as aclick
from commands.scan import scan
from commands.report import report
from commands.reports import reports


@aclick.group(help="CLI tool to access to the filescan service")
def cli():
    pass


cli_groups = aclick.CommandCollection(sources=[scan, report, reports])
if __name__ == "__main__":
    cli_groups(_anyio_backend="asyncio")  # or asyncio, or curio
