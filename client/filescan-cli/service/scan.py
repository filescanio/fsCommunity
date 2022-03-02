from typing import Optional
from core.http import HttpRequests
import os
from .endpoints import get_endpoint, FILE_SCAN
from common.config import API_KEY, USER_AGENT
from core.logger import system_logger


class ScanFile():

    def __init__(self):
        self.http_client = HttpRequests()


    async def run(
        self,
        file: Optional[str],
        link: Optional[str],
        desc: str,
        tags: str,
        prop_tags: bool,
        password: str,
        is_private: bool
    ):
        """Upload a file or link and receive its report"""

        response = await self.__upload(file, link, desc, tags, prop_tags,password, is_private)
        system_logger.success(response)


    async def __upload(
        self,
        file: Optional[str],
        link: Optional[str],
        desc: str,
        tags: str,
        prop_tags: bool,
        password: str,
        is_private: bool
    ):
        """Upload a file for scanning"""

        if file:
            name = os.path.basename(file)
        else:
            name = link[link.rindex('/') + 1:]

        try:
            result = await self.http_client.post_file(
                get_endpoint(FILE_SCAN),
                {
                    'X-Api-Key': API_KEY,
                    'accept': 'application/json',
                    'User-Agent': USER_AGENT
                },
                {
                    'name': name,
                    'path': file,
                    'link': link,
                    'extra': {
                        'description': desc,
                        'tags': tags,
                        'propagate_tags': f'{prop_tags}',
                        'password': password,
                        'is_private': f'{is_private}'
                    }
                }
            )

            return result

        except Exception as ex:
            system_logger.error(f'Error of type {type(ex).__name__} occurred. Arguments:\n{ex.message}')
