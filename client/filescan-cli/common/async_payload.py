from typing import Any, Optional
from aiohttp.payload import Payload
from aiohttp.multipart import MultipartPayloadWriter

class AsyncPayload(Payload):
    """Async payload for multipart writer"""

    def __init__(self,
        value,
        size,
        *args: Any,
        **kwargs: Any
    ) -> None:
        super().__init__(value, *args, **kwargs)
        self.total_size = size


    @property
    def size(self) -> Optional[int]:
        """Size of the payload."""

        return self.total_size


    async def write(self, writer: MultipartPayloadWriter) -> None:
        """Write body."""

        buffers = self._value
        async for chunk in buffers:
            await writer.write(chunk)
