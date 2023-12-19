# Formal requirements (Remember to delete this)

All your documentation for this report is written in Markdown, a lightweight markup language ([https://www.markdownguide.org](https://www.markdownguide.org/)). In the root of your repository, create a directory called `docs`. Let it contain a file called `report.md`.

In case you include illustrations from image files, put all of these of a subdirectory of `docs`, which shall be called `images`.

Before 21/12/23 14:00, hand-in a PDF file with your report on LearnIT ([https://learnit.itu.dk/mod/exam/view.php?id=182186](https://learnit.itu.dk/mod/exam/view.php?id=182186)). The filename of your report has to be `2023_itubdsa_group_<no>_report.pdf`, where where `<no>` is replaced with the group number from [here](https://ituniversity.sharepoint.com/:x:/r/sites/2023AnalysisDesignandSoftwareArchitecture/Shared%20Documents/General/Groups.xlsx?d=w1bf8302469ea4240b490ba7fb3d23ed3&csf=1&web=1&e=TdgQdm).

You can convert your report from markdown to PDF format with `pandoc`:

```
cd docs
pandoc report.md -o 2023_itubdsa_group_<no>_report.pdf
```

Find instructions on how to install `pandoc` [here](https://pandoc.org/installing.html). In case you do not want to install `pandoc` locally to build your report PDF file, you can add the [GitHub Actions Pandoc Action](https://github.com/pandoc/pandoc-action-example) to a respective workflow that updates the PDF file on pushes to/merges with the `docs` directory in your repository.

Use the [report template](https://github.com/itu-bdsa/lecture_notes/blob/main/sessions/session_12/docs/report.md) that is provided next to this description. OBS: You have to adapt the metadata of `report.md` (the block within `---` on top) accordingly!

Note, good project reports are spell- and grammar-checked! This can be done in many different ways. In case you write your report in VSCode, then you might consider the following extensions: for [spell-checking in VSCode](https://marketplace.visualstudio.com/items?itemName=streetsidesoftware.code-spell-checker) and for [spell- and grammar-checking](https://marketplace.visualstudio.com/items?itemName=valentjn.vscode-ltex)

OBS: Work on your documentation in the same way that you work on source code. That is, small iterative and incremental steps with respective commits.

When you have to write something in the project report, write concisely. That is, write short and precise sentences that do not contain fluff. Important, all illustrations have to be legible in the produced PDF document. So make sure they do not become too small.

Create all illustrations either with [PlantUML](https://plantuml.com/) or with [DrawIO](https://app.diagrams.net/). Store all sources of your diagrams, i.e., PlantUML diagram source code or DrawIO XML files under `docs` in a directory called `diagrams`.

Before handing in your project report, make sure that your source code is suitably documented in-code.

**OBS**: After handing in your reports, please let your deployed systems be operational until the end of the third week of January 2024.

The following describes the sections that your report has to provide. These sections are also provided in the project template.

