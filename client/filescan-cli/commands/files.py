import asyncclick as aclick
from flow.scan import ScanFlow

@aclick.group(name='files')
def files():
    pass


@files.command('file', short_help='Download a file')
@aclick.option('--hash', type=str, required=True, is_flag=False, help='File hash to be downloaded')
async def download_file(hash: str):
    
    print(f'{hash}')
