from typing import Dict
from common.config import API_KEY, USER_AGENT


def get_public_header() -> Dict:
    return {
        'accept': 'application/json',
        'User-Agent': USER_AGENT
    }


def get_private_header() -> Dict:
    return {
        'X-Api-Key': API_KEY,
        'accept': 'application/json',
        'User-Agent': USER_AGENT
    }
