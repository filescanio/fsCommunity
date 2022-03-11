import os
from typing import Optional

FILE_READ_BLOCK_SIZE = 5242880  # 5M
USER_AGENT = 'Filescan Client'
API_KEY = os.environ.get('API_KEY', '')
SERVICE_BASE_URL = os.environ.get('SERVICE_BASE_URL', 'https://filescan.io')
# API_KEY = '6YEkWKKV9slHMRJQEXYQSK7ymcyES64UWVQJEIw4'
# SERVICE_BASE_URL = 'http://localhost:8000'
# API_KEY = 'ga4d3P7WlRgkK-4wO_HmfOxJuBiGEtS0eWcs5v4G'
# SERVICE_BASE_URL = 'http://staging.filescan.io'


def load_config(conf: Optional[str] = None):
    pass
