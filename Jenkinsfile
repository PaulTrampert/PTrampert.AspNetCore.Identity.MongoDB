def releaseInfo
def branch
def owner = 'PaulTrampert'
def repo = 'PTrampert.AspNetCore.Identity.MongoDB'
def versionPrefix = 'v'
def githubCreds = 'Github User/Pass'
def githubApi = 'https://api.github.com'

pipeline {
  agent {
    docker {
      image 'microsoft/dotnet:2.1-sdk'
      args '-v $HOME/.dotnet:/.dotnet -v $HOME/.nuget:/.nuget'
    }
  }

  options {
    buildDiscarder(logRotator(numToKeepStr:'5'))
  }

  stages {
    stage('Set branch') {
      when { expression { env.BRANCH_NAME != 'master' } }

      steps {
        script {
          branch = env.BRANCH_NAME
        }
      }
    }

    stage('Build Release Info') {
      steps {
        script {
          releaseInfo = generateGithubReleaseInfo(
            owner,
            repo,
            versionPrefix,
            githubCreds,
            githubApi,
            branch,
            env.BUILD_NUMBER
          )

          echo "Next version is ${releaseInfo.nextVersion().toString()}."
          echo "Changelog:\n${releaseInfo.changelogToMarkdown()}"
        }
      }
    }

    stage('Test') {
      agent any

      steps {
        sh "docker-compose run -v \$HOME/.dotnet:/.dotnet -v \$HOME/.nuget:/.nuget -u \$(id -u):\$(id -g) ptrampert.aspnetcore.identity.mongodb.test"
      }
    }

    stage('Package') {
      steps {
        sh "dotnet pack ${repo}/${repo}.csproj -c Release --no-build /p:Version=${releaseInfo.nextVersion().toString()}"
      }
    }

    stage ('Tag') {
      when { expression { env.BRANCH_NAME == 'master' } }

      steps {
        script {
          publishGithubRelease(
            owner,
            repo,
            releaseInfo,
            versionPrefix,
            githubCreds,
            githubApi
          )
        }
      }
    }

    stage('Publish Pre-Release') {
      when { expression{env.BRANCH_NAME != 'master'} }
      environment {
        API_KEY = credentials('nexus-nuget-apikey')
      }
      steps {
        sh "dotnet nuget push **/*.nupkg -s 'https://packages.ptrampert.com/repository/nuget-prereleases/' -k ${env.API_KEY}"
      }
    }

    stage('Publish Release') {
      when { expression {env.BRANCH_NAME == 'master'} }
      environment {
        API_KEY = credentials('nuget-api-key')
      }
      steps {
        sh "dotnet nuget push **/*.nupkg -s https://api.nuget.org/v3/index.json -k ${env.API_KEY}"
      }
    }
  }

  post {
    failure {
      mail(
        to: 'paul.trampert@ptrampert.com',
        subject: "Build status of ${env.JOB_NAME} changed to ${currentBuild.result}", body: "Build log may be found at ${env.BUILD_URL}"
      )
    }
    always {
      archiveArtifacts '**/*.nupkg'

      xunit(
        testTimeMargin: '3000',
        thresholdMode: 1,
        thresholds: [
          failed(unstableThreshold: '0')
        ],
        tools: [
          MSTest(
            deleteOutputFiles: true,
            failIfNotNew: true,
            pattern: '**/*.trx',
            skipNoTestFiles: false,
            stopProcessingIfError: true
          )
        ]
      )

      cobertura(
        autoUpdateHealth: false,
        autoUpdateStability: false,
        coberturaReportFile: '**/*.cobertura.xml',
        conditionalCoverageTargets: '70, 0, 0',
        failUnhealthy: false,
        failUnstable: false,
        lineCoverageTargets: '80, 0, 0',
        maxNumberOfBuilds: 0,
        methodCoverageTargets: '80, 0, 0',
        onlyStable: false,
        sourceEncoding: 'ASCII',
        zoomCoverageChart: false
      )
    }
  }
}