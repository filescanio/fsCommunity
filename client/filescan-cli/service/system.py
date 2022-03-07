import json
from common.config import USER_AGENT
from core.http import HttpRequests
from common.utils import run_safe
from .endpoints import get_endpoint, SYSTEM_INFO, SYSTEM_CONFIG


class System:
    
    def __init__(self):
        self.http_client = HttpRequests()
        self.headers = {
            'accept': 'application/json',
            'User-Agent': USER_AGENT
        }


    async def get_info(self):

        endpoint = get_endpoint(SYSTEM_INFO)
        result = await run_safe(
            self.http_client.get,
            endpoint,
            headers=self.headers
        )

        return json.loads(result)


    async def get_config(self):

        endpoint = get_endpoint(SYSTEM_CONFIG)
        result = await run_safe(
            self.http_client.get,
            endpoint,
            headers=self.headers
        )

        return json.loads(result)
