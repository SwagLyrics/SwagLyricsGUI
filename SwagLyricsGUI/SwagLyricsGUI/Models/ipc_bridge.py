from swaglyrics import cli
from SwSpotify import spotify

song,artist = spotify.current()
print(cli.get_lyrics(song,artist), flush=True)
