import asyncio

async def run_async(func, *args, executor=None):
    """Runs an action in an asyncronous executor"""

    return await asyncio.get_event_loop().run_in_executor(executor, func, *args)
