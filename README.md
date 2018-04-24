# Splitter

Splitter is a Youtube Video splitter that it intended to be used for long timestamped videos. Splitter takes meta information provided in the video description and extracts the audio for each portion of the video.

For example given the following long video: https://www.youtube.com/watch?v=ppzcjw2Xq1Y which has timestamps in the description like the following. Splitter will extract the audio and then slice each portion into it's own audio file whilst retaining meta information.

```
Tracklist
1.- 00:00 Salamander
2.- 02:13 Mahou Hatsudou
3.- 03:56 Dragon Force
4.- 06:14 Prelude to Destruction
5.- 08:37 Mystogan no Theme
6.- 11:14 Dragon Fight
7.- 12:48 Mahou Taisen
8.- 14:24 Natsu no Theme
9.- 16:15 Dragon Civil War
10.- 18:17 Erza no Theme
11.- 21:06 Endless Battle
12.- 23:07 Fairy Tail Main Theme
```
## Timestamp Support

Splitter currently supports the following timestamp formats:

Space Separated:
- `0:00 TRACKNAME`
- `00:00 TRACKNAME`
- `0:00:00 TRACKNAME`
- `00:00:00 TRACKNAME`

Dash Separated:
- `0:00-TRACKNAME`
- `00:00-TRACKNAME`
- `0:00:00-TRACKNAME`
- `00:00:00-TRACKNAME`

If any of these formats are found within a given video's description then Splitter will treat it as a portion of the video with a label and slice that section.

## Dependencies

*ffmpeg is required to be installed on the machine that is running this application.*